using System;
using System.Diagnostics;
using System.IO;
using ApiExampleProject.IntegrationTests.Configuration;

namespace ApiExampleProject.IntegrationTests.TestFixtures
{
    public abstract class BaseTestFixture
        : IDisposable
    {
        private readonly Process funcHostProcess;

        public BaseTestFixture(string functionAppPath, int portNumber)
        {
            var functionHostPath = Environment.ExpandEnvironmentVariables(ConfigurationHelper.Settings.FunctionHostPath);
            var functionAppFolder = Path.GetRelativePath(Directory.GetCurrentDirectory(), functionAppPath);

            funcHostProcess = new Process()
            {
                StartInfo =
                {
                    FileName = functionHostPath,
                    Arguments = $"start -p {portNumber}",
                    WorkingDirectory = functionAppFolder
                }
            };

            var success = funcHostProcess.Start();
            if (!success)
            {
                throw new InvalidOperationException(IntegrationTestResources.CouldNotStart_ErrorMessage);
            }

            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                funcHostProcess.Close();
                funcHostProcess.Dispose();
            }
        }
    }
}
