using Microsoft.JSInterop;

namespace DarwinCMS.Shared.UI
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    /// <summary>
    /// Provides an example of how JavaScript functionality can be wrapped in a .NET class for easy consumption.
    /// </summary>
    /// <remarks>This class loads the associated JavaScript module on demand when first needed. It can be
    /// registered as a  scoped dependency injection (DI) service and injected into Blazor components for use.</remarks>
    public class ExampleJsInterop : IAsyncDisposable
    {
        /// <summary>
        /// Represents a lazily initialized task that retrieves a JavaScript module reference.
        /// </summary>
        /// <remarks>This field is intended to store a lazy task that asynchronously loads a JavaScript
        /// module. The task is initialized only when accessed, ensuring that the module is loaded on demand.</remarks>
        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleJsInterop"/> class,  which provides JavaScript interop
        /// functionality for the associated module.
        /// </summary>
        /// <remarks>This constructor sets up the interop by importing the JavaScript module  located at
        /// "./_content/DarwinCMS.Shared.UI/exampleJsInterop.js". The module  is loaded asynchronously and cached for
        /// future use.</remarks>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/> instance used to invoke JavaScript functions.</param>
        public ExampleJsInterop(IJSRuntime jsRuntime)
        {
            _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/DarwinCMS.Shared.UI/exampleJsInterop.js").AsTask());
        }

        /// <summary>
        /// Displays a prompt dialog with the specified message and returns the user's input.
        /// </summary>
        /// <remarks>This method invokes a JavaScript function named "showPrompt" to display the prompt
        /// dialog.  Ensure that the JavaScript module containing the "showPrompt" function is properly loaded and
        /// accessible.</remarks>
        /// <param name="message">The message to display in the prompt dialog.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> representing the asynchronous operation.  The result contains the user's
        /// input as a string, or <see langword="null"/> if the prompt was canceled.</returns>
        public async ValueTask<string> Prompt(string message)
        {
            var module = await _moduleTask.Value;
            return await module.InvokeAsync<string>("showPrompt", message);
        }

        /// <summary>
        /// Asynchronously releases the resources used by the current instance.
        /// </summary>
        /// <remarks>This method disposes of any resources associated with the instance, including
        /// asynchronously disposing of the underlying module if it has been initialized. It should be called when the
        /// instance is no longer needed to ensure proper cleanup.</remarks>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous dispose operation.</returns>
        public async ValueTask DisposeAsync()
        {
            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
