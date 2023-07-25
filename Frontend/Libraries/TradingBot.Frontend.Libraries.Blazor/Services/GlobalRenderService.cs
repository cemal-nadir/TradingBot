using CNG.Http.Responses;

namespace TradingBot.Frontend.Libraries.Blazor.Services
{
	public sealed class GlobalRenderService
	{
		public event EventHandler<int>? ForceRender;
		public event EventHandler? ForceRenderAll;
		public event EventHandler<ExceptionResponse>? ExceptionRender; 

		public void Render(int cid)
		{
			ForceRender?.Invoke(this, cid);
		}

		public void RenderAll()
		{
			ForceRenderAll?.Invoke(this,EventArgs.Empty);
		}

		public void RenderException(ExceptionResponse exceptionResponse)
		{
			ExceptionRender?.Invoke(this,exceptionResponse);
		}

	}
}
