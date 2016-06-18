﻿using Douban.Contract;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Douban.Spider
{
    public abstract class SpiderDirector<T> : ISpider where T : class
    {
        protected volatile bool _stop = false;
        protected static Logger _log = LogManager.GetCurrentClassLogger();
        protected IList<T> _availableTask = null;

        protected abstract IList<T> GetAvailableTasks();

        protected virtual IList<T> InitialWork()
        {
            return new List<T>();
        }

        protected IList<T> WrapDoingWork(int index, ParallelLoopState state, IList<T> tasks)
        {
            try
            {
                if (IsStop())
                {
                    _log.Warn("Stopping spider work...");
                    state.Stop();
                }
                else if (state.IsStopped)
                {
                    _log.Warn("Stopped...");
                }
                else
                {
                    DoingWork(_availableTask[index]);
                    tasks.Add(_availableTask[index]);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Task Error: ", _availableTask[index]);
            }
            return tasks;
        }

        protected abstract void DoingWork(T task);

        protected abstract void DoneWork(IList<T> tasks);

        protected virtual void WrapDoneWork(IList<T> tasks)
        {
            try
            {
                DoneWork(tasks);
                if (OnProceed != null)
                {
                    OnProceed(this, tasks.Count);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Done work error");

                if (tasks != null)
                {
                    foreach (T t in tasks)
                        _log.Error(t.ToString());
                }
            }
    }

        protected virtual bool IsStop()
        {
            return _stop;
        }

        public virtual void Restart()
        {
            _stop = false;
            Run();
        }

        public virtual void Run()
        {
            while (!IsStop())
            {
                _availableTask = GetAvailableTasks();
                if (_availableTask == null || _availableTask.Count == 0)
                    _stop = true;
                else
                    Parallel.For(0, _availableTask.Count, InitialWork, WrapDoingWork, WrapDoneWork);
            }

            if (OnCompleted != null)
            {
                OnCompleted(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            _stop = true;
        }

        public event EventHandler<int> OnProceed;
        public event EventHandler OnCompleted;
    }
}
