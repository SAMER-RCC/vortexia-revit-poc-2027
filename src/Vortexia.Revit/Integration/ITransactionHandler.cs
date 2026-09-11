namespace Vortexia.Revit.Integration
{
    using Autodesk.Revit.DB;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the interface for handling Revit transactions.
    /// </summary>
    public interface ITransactionHandler
    {
        /// <summary>
        /// Executes an action within a transaction asynchronously.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="transactionName">The name of the transaction.</param>
        /// <param name="action">The action to execute within the transaction.</param>
        /// <returns>A task that returns the result of the action.</returns>
        Task<T> ExecuteInTransactionAsync<T>(
            string transactionName,
            Func<Transaction, Task<T>> action);

        /// <summary>
        /// Executes an action within a transaction asynchronously.
        /// </summary>
        /// <param name="transactionName">The name of the transaction.</param>
        /// <param name="action">The action to execute within the transaction.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteInTransactionAsync(
            string transactionName,
            Func<Transaction, Task> action);

        /// <summary>
        /// Executes an action within a transaction group asynchronously.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="groupName">The name of the transaction group.</param>
        /// <param name="action">The action to execute within the transaction group.</param>
        /// <returns>A task that returns the result of the action.</returns>
        Task<T> ExecuteInTransactionGroupAsync<T>(
            string groupName,
            Func<TransactionGroup, Task<T>> action);
    }

    /// <summary>
    /// Default implementation of the transaction handler.
    /// </summary>
    public class TransactionHandler : ITransactionHandler
    {
        private readonly Document _document;
        private readonly ILogger<TransactionHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the TransactionHandler.
        /// </summary>
        /// <param name="document">The Revit document.</param>
        /// <param name="logger">The logger instance.</param>
        public TransactionHandler(
            Document document,
            ILogger<TransactionHandler> logger)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Executes an action within a transaction asynchronously.
        /// </summary>
        public async Task<T> ExecuteInTransactionAsync<T>(
            string transactionName,
            Func<Transaction, Task<T>> action)
        {
            if (string.IsNullOrWhiteSpace(transactionName))
            {
                throw new ArgumentNullException(nameof(transactionName));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (var transaction = new Transaction(_document, transactionName))
            {
                try
                {
                    _logger.LogInformation("Starting transaction: {TransactionName}", transactionName);
                    
                    transaction.Start();
                    var result = await action(transaction);
                    transaction.Commit();
                    
                    _logger.LogInformation("Transaction completed successfully: {TransactionName}", transactionName);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in transaction {TransactionName}: {Message}", transactionName, ex.Message);
                    
                    if (transaction.HasStarted())
                    {
                        transaction.RollBack();
                    }
                    
                    throw;
                }
            }
        }

        /// <summary>
        /// Executes an action within a transaction asynchronously.
        /// </summary>
        public async Task ExecuteInTransactionAsync(
            string transactionName,
            Func<Transaction, Task> action)
        {
            if (string.IsNullOrWhiteSpace(transactionName))
            {
                throw new ArgumentNullException(nameof(transactionName));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (var transaction = new Transaction(_document, transactionName))
            {
                try
                {
                    _logger.LogInformation("Starting transaction: {TransactionName}", transactionName);
                    
                    transaction.Start();
                    await action(transaction);
                    transaction.Commit();
                    
                    _logger.LogInformation("Transaction completed successfully: {TransactionName}", transactionName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in transaction {TransactionName}: {Message}", transactionName, ex.Message);
                    
                    if (transaction.HasStarted())
                    {
                        transaction.RollBack();
                    }
                    
                    throw;
                }
            }
        }

        /// <summary>
        /// Executes an action within a transaction group asynchronously.
        /// </summary>
        public async Task<T> ExecuteInTransactionGroupAsync<T>(
            string groupName,
            Func<TransactionGroup, Task<T>> action)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                throw new ArgumentNullException(nameof(groupName));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (var transactionGroup = new TransactionGroup(_document, groupName))
            {
                try
                {
                    _logger.LogInformation("Starting transaction group: {GroupName}", groupName);
                    
                    transactionGroup.Start();
                    var result = await action(transactionGroup);
                    transactionGroup.Assimilate();
                    
                    _logger.LogInformation("Transaction group completed successfully: {GroupName}", groupName);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in transaction group {GroupName}: {Message}", groupName, ex.Message);
                    
                    if (transactionGroup.IsOpen)
                    {
                        transactionGroup.RollBack();
                    }
                    
                    throw;
                }
            }
        }
    }
}
