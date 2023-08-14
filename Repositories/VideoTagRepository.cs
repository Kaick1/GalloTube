using System.Data;
using GalloTube.Interfaces;
using GalloTube.Models;
using MySql.Data.MySqlClient;

namespace GalloTube.Repositories;

public class MovieTagRepository : IMovieTagRepository
{
    readonly string connectionString = "server=localhost;port=3306;database=GalloFlixdb;uid=root;pwd=''";

    public void Create(int MovieId, byte TagId)
    {
        MySqlConnection connection = new(connectionString);
        string sql = "insert into MovieTag(MovieId, TagId) values (@MovieId, @TagId)";
        MySqlCommand command = new(sql, connection)
        {
            CommandType = CommandType.Text
        };
        command.Parameters.AddWithValue("@MovieId", MovieId);
        command.Parameters.AddWithValue("@TagId", TagId);
        
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }

    public void Delete(int MovieId, byte TagId)
    {
        MySqlConnection connection = new(connectionString);
        string sql = "delete from MovieTag where MovieId = @MovieId and TagId = @TagId";
        MySqlCommand command = new(sql, connection)
        {
            CommandType = CommandType.Text
        };
        command.Parameters.AddWithValue("@MovieId", MovieId);
        command.Parameters.AddWithValue("@TagId", TagId);
        
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }

    public void Delete(int MovieId)
    {
        MySqlConnection connection = new(connectionString);
        string sql = "delete from MovieTag where MovieId = @MovieId";
        MySqlCommand command = new(sql, connection)
        {
            CommandType = CommandType.Text
        };
        command.Parameters.AddWithValue("@MovieId", MovieId);
        
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }

    public List<Tag> ReadTagsByMovie(int MovieId)
    {
        MySqlConnection connection = new(connectionString);
        string sql = "select * from tag where id in "
                   + "(select TagId from MovieTag where MovieId = @MovieId)";
        MySqlCommand command = new(sql, connection)
        {
            CommandType = CommandType.Text
        };
        command.Parameters.AddWithValue("@MovieId", MovieId);
        
        List<Tag> tags = new();
        connection.Open();
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Tag tag = new()
            {
                Id = reader.GetByte("id"),
                Name = reader.GetString("name")
            };
            tags.Add(tag);
        }
        connection.Close();
        return tags;
    }

    public List<MovieTag> ReadMovieTag()
    {
        MySqlConnection connection = new(connectionString);
        string sql = "select * from MovieTag";
        MySqlCommand command = new(sql, connection)
        {
            CommandType = CommandType.Text
        };
        
        List<MovieTag> movieTags = new();
        connection.Open();
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            MovieTag movieTag = new()
            {
                MovieId = reader.GetInt32("MovieId"),
                TagId = reader.GetByte("TagId")
            };
            movieTags.Add(movieTag);
        }
        connection.Close();
        return movieTags;
    }

    public List<Movie> ReadMoviesByTag(byte TagId)
    {
        MySqlConnection connection = new(connectionString);
        string sql = "select * from movie where id in "
                   + "(select MovieId from movietag where TagId = @TagId)";
        MySqlCommand command = new(sql, connection)
        {
            CommandType = CommandType.Text
        };
        command.Parameters.AddWithValue("@TagId", TagId);
        
        List<Movie> movies = new();
        connection.Open();
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Movie movie = new()
            {
                Id = reader.GetInt32("id"),
                Title = reader.GetString("title"),
                OriginalTitle = reader.GetString("originalTitle"),
                Synopsis = reader.GetString("synopsis"),
                MovieYear = reader.GetInt16("movieYear"),
                Duration = reader.GetInt16("duration"),
                AgeRating = reader.GetByte("ageRating"),
                Image = reader.GetString("image")
            };
            movies.Add(movie);
        }
        connection.Close();
        return movies;
    }
}