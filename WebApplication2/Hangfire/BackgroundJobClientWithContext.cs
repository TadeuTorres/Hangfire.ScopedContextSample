using System;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.States;

namespace WebApplication2.Hangfire
{
    public class BackgroundJobClientWithContext : IBackgroundJobClientWithContext
    {
        private readonly JobStorage _storage;
        private readonly IBackgroundJobFactory _factory;
        private readonly IBackgroundJobStateChanger _stateChanger;

        public BackgroundJobClientWithContext()
            : this(JobStorage.Current)
        {
        }

        public BackgroundJobClientWithContext([NotNull] JobStorage storage)
            : this(storage, JobFilterProviders.Providers)
        {
        }

        public BackgroundJobClientWithContext([NotNull] JobStorage storage, [NotNull] IJobFilterProvider filterProvider)
            : this(storage, new BackgroundJobFactory(filterProvider), new BackgroundJobStateChanger(filterProvider))
        {
        }

        public BackgroundJobClientWithContext(
            [NotNull] JobStorage storage,
            [NotNull] IBackgroundJobFactory factory,
            [NotNull] IBackgroundJobStateChanger stateChanger)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _stateChanger = stateChanger ?? throw new ArgumentNullException(nameof(stateChanger));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public bool ChangeState([NotNull] string jobId, [NotNull] IState state, [CanBeNull] string expectedState)
        {
            if (jobId == null) throw new ArgumentNullException(nameof(jobId));
            if (state == null) throw new ArgumentNullException(nameof(state));

            try
            {
                using (var connection = _storage.GetConnection())
                {
                    var appliedState = _stateChanger.ChangeState(new StateChangeContext(
                        _storage,
                        connection,
                        jobId,
                        state,
                        expectedState != null ? new[] { expectedState } : null));

                    return appliedState != null && appliedState.Name.Equals(state.Name, StringComparison.OrdinalIgnoreCase);
                }
            }
            catch (Exception ex)
            {
                throw new BackgroundJobClientException("State change of a background job failed. See inner exception for details", ex);
            }
        }

        public string Create([NotNull]Job job, [NotNull]IState state, object context)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));
            if (state == null) throw new ArgumentNullException(nameof(state));

            try
            {
                using (var connection = _storage.GetConnection())
                {
                    var createContext = new CreateContext(_storage, connection, job, state);
                    createContext.Parameters.Add("HangfireContext", context);

                    var backroundJob = _factory.Create(createContext);

                    return backroundJob?.Id;
                }
            }
            catch (Exception ex)
            {
                throw new BackgroundJobClientException("Background job creation failed. See inner exception for details.", ex);
            }
        }

        public string Create([NotNull] Job job, [NotNull] IState state)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));
            if (state == null) throw new ArgumentNullException(nameof(state));

            try
            {
                using (var connection = _storage.GetConnection())
                {
                    var context = new CreateContext(_storage, connection, job, state);
                    var backroundJob = _factory.Create(context);

                    return backroundJob?.Id;
                }
            }
            catch (Exception ex)
            {
                throw new BackgroundJobClientException("Background job creation failed. See inner exception for details.", ex);
            }
        }
    }
}