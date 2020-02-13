using System;
using System.Diagnostics;
using System.IO;
using ApiExampleProject.IntegrationTests.Configuration;

namespace ApiExampleProject.IntegrationTests.TestFixtures
{
    public class IntegrationTestFixture
        : IDisposable
    {
        private readonly Process funcHostProcess;

        public int Port { get; } = 7001;

        public IntegrationTestFixture()
        {
            var functionHostPath = Environment.ExpandEnvironmentVariables(ConfigurationHelper.Settings.FunctionHostPath);
            var functionAppFolder = Path.GetRelativePath(Directory.GetCurrentDirectory(), ConfigurationHelper.Settings.FunctionApplicationPath);

            funcHostProcess = new Process()
            {
                StartInfo =
                {
                    FileName = functionHostPath,
                    Arguments = $"start -p {Port}",
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
                if (!funcHostProcess.HasExited)
                {
                    funcHostProcess.Kill();
                }

                funcHostProcess.Kill();
                funcHostProcess.Dispose();
            }
        }
    }
}
