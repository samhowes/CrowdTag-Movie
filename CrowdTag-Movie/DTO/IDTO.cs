using System;
namespace CrowdTagMovie.DTO
{
	public interface IDTO
	{
		int ID {get; set;}
		void UpdateEntity(ref object entity);
	}
}
