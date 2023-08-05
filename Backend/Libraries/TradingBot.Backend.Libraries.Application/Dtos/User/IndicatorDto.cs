
using MongoDB.Bson;

namespace TradingBot.Backend.Libraries.Application.Dtos.User
{
	public class IndicatorDto
	{
		private string? _id;

		public string? Id
		{
			get => _id;
			set => _id = string.IsNullOrEmpty(value) ? ObjectId.GenerateNewId().ToString() : value;
		}
		public string? Name { get; set; }
		public string? Description { get; set; }
		public bool IsActive { get; set; }
	}
}
