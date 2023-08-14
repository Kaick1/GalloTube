using System.Data;
using GalloTube.Interfaces;
using GalloTube.Models;
using MySql.Data.MySqlClient;

namespace GalloTube.Repositories;

public class VideoRepository : IVideoRepository
{
    readonly string connectionString = "server=localhost;port=3306;database=GalloTubedb;uid=root;pwd=''";
    readonly IVideoTagRepository _videoTagRepository;

    public VideoRepository(IVideoTagRepository videoTagRepository)
    {
        _videoTagRepository = videoTagRepository;
    }


    public void Create(Video model)
    {
        MySqlConnection connection = new(connectionString);
        string sql = "insert into Video(Title, OriginalTitle, Synopsis, VideoYear, Duration, AgeRating, Image) "
              + "values (@Title, @OriginalTitle, @Synopsis, @VideoYear, @Duration, @AgeRating, @Image)";
        MySqlCommand command = new(sql, connection)
        {
            CommandType = CommandType.Text
        };
        command.Parameters.AddWithValue("@Title", model.Title);
        command.Parameters.AddWithValue("@OriginalTitle", model.OriginalTitle);
        command.Parameters.AddWithValue("@Synopsis", model.Synopsis);
        command.Parameters.AddWithValue("@VideoYear", model.VideoYear);
        command.Parameters.AddWithValue("@Duration", model.Duration);
        command.Parameters.AddWithValue("@AgeRating", model.AgeRating);
        command.Parameters.AddWithValue("@Image", model.Image);
        
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }

    public void Delete(int? id)
    {
        MySqlConnection connection = new(connectionString);
        string sql = "delete from Video where Id = @Id";
        MySqlCommand command = new(sql, connection)
        {
            CommandType = CommandType.Text
        };
        command.Parameters.AddWithValue("@Id", id);
        
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }

    public List<Video> ReadAll()
    {
        MySqlConnection connection = new(connectionString);
        string sql = "select * from Video";
        MySqlCommand command = new(sql, connection)
        {
            CommandType = CommandType.Text
        };
        
        List<Video> Videos = new();
        connection.Open();
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Video Video = new()
            {
                Id = reader.GetInt32("id"),
                Title = reader.GetString("title"),
                OriginalTitle = reader.GetString("originalTitle"),
                Synopsis = reader.GetString("synopsis"),
                VideoYear = reader.GetInt16("videoYear"),
                Duration = reader.GetInt16("duration"),
                AgeRating = reader.GetByte("ageRating"),
                Image = reader.GetString("image")
            };
            Videos.Add(Video);
        }
        connection.Close();
        return Videos;
    }

    public Video ReadById(int? id)
    {
        MySqlConnection connection = new(connectionString);
        string sql = "select * from Video where Id = @Id";
        MySqlCommand command = new(sql, connection)
        {
            CommandType = CommandType.Text
        };
        command.Parameters.AddWithValue("@Id", id);
        
        connection.Open();
        MySqlDataReader reader = command.ExecuteReader();
        reader.Read();
        if (reader.HasRows)
        {
            Video Video = new()
            {
                Id = reader.GetInt32("id"),
                Title = reader.GetString("title"),
                OriginalTitle = reader.GetString("originalTitle"),
                Synopsis = reader.GetString("synopsis"),
                VideoYear = reader.GetInt16("videoYear"),
                Duration = reader.GetInt16("duration"),
                AgeRating = reader.GetByte("ageRating"),
                Image = reader.GetString("image")
            };
            connection.Close();
            return Video;
        }
        connection.Close();
        return null;
    }

    public void Update(Video model)
    {
        MySqlConnection connection = new(connectionString);
        string sql = "update Video set "
                        + "Title = @Title, "
                        + "OriginalTitle = @OriginalTitle, "
                        + "Synopsis = @Synopsis, "
                        + "VideoYear = @VideoYear, "
                        + "Duration = @Duration, "
                        + "AgeRating = @AgeRating, "
                        + "Image = @Image "
                    + "where Id = @Id";
        MySqlCommand command = new(sql, connection)
        {
            CommandType = CommandType.Text
        };
        command.Parameters.AddWithValue("@Id", model.Id);
        command.Parameters.AddWithValue("@Title", model.Title);
        command.Parameters.AddWithValue("@OriginalTitle", model.OriginalTitle);
        command.Parameters.AddWithValue("@Synopsis", model.Synopsis);
        command.Parameters.AddWithValue("@VideoYear", model.VideoYear);
        command.Parameters.AddWithValue("@Duration", model.Duration);
        command.Parameters.AddWithValue("@AgeRating", model.AgeRating);
        command.Parameters.AddWithValue("@Image", model.Image);
        
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }

    public List<Video> ReadAllDetailed()
    {
        List<Video> Videos = ReadAll();
        foreach (Video video in Videos)
        {
            video.Tags = _videoTagRepository.ReadTagsByVideo(video.Id);
        }
        return Videos;
    }

    public Video ReadByIdDetailed(int id)
    {
        Video video = ReadById(id);
        video.Tags = _videoTagRepository.ReadTagsByVideo(video.Id);
        return video;
    }
}
