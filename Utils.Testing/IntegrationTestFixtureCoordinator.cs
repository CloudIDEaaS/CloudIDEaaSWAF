using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Utils
{
    public class IntegrationTestFixtureCoordinator : IIntegrationTestFixtureCoordinator
    {
        private IIntegrationTestFixtureBase[] fixtures;
        private Queue<FixtureClosure> fixtureClosures;

        public List<IIntegrationTestFixtureBase> TestFixtures { get; }

        public IntegrationTestFixtureCoordinator()
        {
            this.TestFixtures = new List<IIntegrationTestFixtureBase>();
            fixtureClosures = new Queue<FixtureClosure>();
        }

        public void SetShutdownOrder(params IIntegrationTestFixtureBase[] fixtures)
        {
            this.fixtures = fixtures;

            foreach (var fixture in fixtures)
            {
                if (!fixtureClosures.Any(f => f.Fixture == fixture))
                {
                    fixtureClosures.Enqueue(new FixtureClosure(fixture));
                }
            }
        }

        public Task<HttpResponseMessage> RouteRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            foreach (var fixture in this.TestFixtures)
            {
                if (fixture.CanHandle(request))
                {
                    return fixture.HandleSendAsync(request, cancellationToken);
                }
            }

            throw new NotImplementedException($"No test fixure registered for request to { request.RequestUri.AbsoluteUri }");
        }

        public void Wait(IIntegrationTestFixtureBase integrationTestFixture, Action action)
        {
            if (fixtureClosures.Count > 0)
            {
                var closure = fixtureClosures.Peek();

                if (closure.Fixture == integrationTestFixture)
                {
                    closure = fixtureClosures.Dequeue();
                    action();

                    while (fixtureClosures.Count > 0)
                    {
                        closure = fixtureClosures.Peek();

                        if (closure.Action != null)
                        {
                            fixtureClosures.Dequeue();
                            closure.Action();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    closure = this.fixtureClosures.Single(c => c.Fixture == integrationTestFixture);

                    closure.Action = action;
                }
            }
            else
            {
                action();
            }
        }

        public ManualResetEvent SignalStart()
        {
            throw new NotImplementedException();
        }
    }
}
