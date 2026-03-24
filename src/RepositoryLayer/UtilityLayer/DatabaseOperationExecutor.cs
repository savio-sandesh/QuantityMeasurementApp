using RepositoryLayer.Exceptions;
using System;

namespace RepositoryLayer
{
    internal static class DatabaseOperationExecutor
    {
        public static T Execute<T>(Func<T> operation, string errorMessage)
        {
            try
            {
                return operation();
            }
            catch (DatabaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(errorMessage, ex);
            }
        }

        public static void Execute(Action operation, string errorMessage)
        {
            try
            {
                operation();
            }
            catch (DatabaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(errorMessage, ex);
            }
        }
    }
}