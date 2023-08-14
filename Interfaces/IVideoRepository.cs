using Gallotube.Models;

namespace GalloTube.Interfaces;

public interface IVideoRepository : IRepository<Movie>
{
    List<Video> ReadAllDetailed();

    Video ReadByIdDetailed(int id);
}

