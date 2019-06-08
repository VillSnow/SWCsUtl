using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

#nullable enable

namespace SWCsUtl {
	public class CollectionSubject<T> :
		ObservableCollection<T>,
		IObserver<IEnumerable<T>>,
		IObservable<IEnumerable<T>> {

		public IEqualityComparer<T> Comparer { get; set; } = EqualityComparer<T>.Default;


		public delegate void AssignItemDelegate(T existingItem, T newItem);
		public delegate void SendToUIDelegate(Action action);

		public AssignItemDelegate AssignItem { get; set; } = (T dst, T src) => { };

		public SendToUIDelegate Dispatch { get; set; } = (Action action) => action();

		/// <summary>
		/// CollectionSubject&lt;T&gt; calls Assign(FallbackValue) if OnError was called and FallbackValue was not null.
		/// </summary>
		public IEnumerable<T>? FallbackValue { get; set; } = null;


		bool isClosed = false;

		readonly List<IObserver<IEnumerable<T>>> observers = new List<IObserver<IEnumerable<T>>>();


		public void Assign(IEnumerable<T> source) {


			var source_ = CollectionTools.EnsureIsReadOnlyList(source);

			if (source_.Count == 0) {
				Dispatch(() => Clear());
				return;
			}

			int i;
			for (i = 0; i < Count && i < source_.Count; ++i) {

				if (Comparer.Equals(this[i], source_[i])) {
					Dispatch(() => AssignItem(this[i], source_[i]));
					continue;
				}

				// find existing
				int pos = this.IndexOf(x => Comparer.Equals(x, source_[i]));
				if (i < pos) {
					// move if exists and not scanned yet
					Dispatch(() => Move(pos, i));
					Dispatch(() => AssignItem(this[i], source_[i]));
					continue;
				}

				// if the scanning would be used later
				if (source_.Contains(this[i], Comparer)) {
					// keep the scanning
					Dispatch(() => Insert(i, source_[i]));
				} else {
					// overwite the scanning
					Dispatch(() => SetItem(i, source_[i]));
				}


			}

			// add rest
			for (; i < source_.Count; ++i) {
				Dispatch(() => Add(source_[i]));
			}

			// remove tail
			while (i < Count) {
				Dispatch(() => RemoveAt(i));
			}
		}

		public void OnNext(IEnumerable<T> value) {
			if (!isClosed) { Assign(value); }
		}

		public void OnCompleted() {
			isClosed = true;
		}

		public void OnError(Exception error) {
			isClosed = true;
			if (FallbackValue is object) { Assign(FallbackValue); }
		}

		public IDisposable Subscribe(IObserver<IEnumerable<T>> observer) {
			observers.Add(observer);
			return new Disposable(this, observer);
		}

		public void Unsubscribe(IObserver<IEnumerable<T>> observer) {
			observers.RemoveAll(observer.Equals);
		}

		class Disposable : IDisposable {
			readonly WeakReference<CollectionSubject<T>> subject;
			readonly WeakReference<IObserver<IEnumerable<T>>> observer;
			bool disposed = false;

			public Disposable(CollectionSubject<T> subject, IObserver<IEnumerable<T>> observer) {
				this.subject = new WeakReference<CollectionSubject<T>>(subject);
				this.observer = new WeakReference<IObserver<IEnumerable<T>>>(observer);
			}

			public void Dispose() {
				if (
					!disposed &&
					subject.TryGetTarget(out var subject1) &&
					observer.TryGetTarget(out var observer1)
				) {
					subject1.Unsubscribe(observer1);
				}
				disposed = true;
			}
		}


	}
}
