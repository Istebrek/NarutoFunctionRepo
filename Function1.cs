using AnimeFunctionCode.Data;
using AnimeFunctionCode.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AnimeFunctionCode;

public class Function1
{
    private readonly ILogger<Function1> _logger;
    static NarutoCRUD db = new NarutoCRUD("NarutoDb");


    public Function1(ILogger<Function1> logger)
    {
        _logger = logger;
    }

    //All functions get triggered by get and post requests.

    //This function uses a get-method to read all the data from the database and returns it as a list formatted as JSON.
    //Endpoint: /api/GetCharacters?code=<your-function-key>
    [Function("GetFunction")]
    public async Task GetCharactersRun([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "GetCharacters")] HttpRequest req)
    {
        var response = req.HttpContext.Response;

        try
        {
            _logger.LogInformation("Fetching all characters. Hopefully this isn’t another filler arc.");

            var characters = await db.GetCharacters("Characters");

            if (characters == null || !characters.Any())
            {
                _logger.LogWarning("No characters found. What is this, Sakura’s fan meetup?");
                response.StatusCode = StatusCodes.Status500InternalServerError;

            }
            await response.WriteAsJsonAsync(characters);
        } catch (Exception ex)
        {
            _logger.LogError($"Failed to retrieve characters. Might be trapped in a genjutsu or just a poorly written plot twist. Error message {ex.Message}");
            response.StatusCode = StatusCodes.Status500InternalServerError;
            await response.WriteAsJsonAsync($"Failed to retrieve characters. Might be trapped in a genjutsu or just a poorly written plot twist. Error message { ex.Message}");
        }
    }

    //This function uses a post-method to post a new character to the database and returns the new character formatted as JSON.
    //Endpoint: /api/PostCharacter?code=<your-function-key>
    //Request-body in Postman: { "name": "string", "village": "string", "rating": "int", "jutsu": "string" }
    //Parameters in web-browser: /api/PostCharacter?code=<your-function-key>&name=string&village=string&rating=int&jutsu=string
    [Function("PostFunction")]
    public async Task PostCharacterRun([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "PostCharacter")] HttpRequest req,
        string name, string? village, int? rating, string? jutsu)
    {
        var response = req.HttpContext.Response;

        try
        {
            _logger.LogInformation($"Trying to add {name}... because clearly the world needs more overpowered anime characters.");

            NarutoCharacters character = new NarutoCharacters
            {
                Name = name,
                Village = village,
                Rating = rating,
                Jutsu = jutsu
            };
            if (character == null)
            {
                _logger.LogWarning("Character was not added, they used transportation jutsu.");
                response.StatusCode = StatusCodes.Status500InternalServerError;

            }
            db.AddCharacter("Characters", character);
            await response.WriteAsJsonAsync(character);
            Console.WriteLine($"{name} was added. Unlike Naruto’s dad, this character showed up.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to add {name}. It’s like trying to teach Sasuke teamwork. {ex.Message}");
            response.StatusCode = StatusCodes.Status500InternalServerError;
            await response.WriteAsJsonAsync($"Failed to add {name}. It’s like trying to teach Sasuke teamwork. {ex.Message}");
        }
    }

    //This function uses a get-method to read and return the data of a character formatted as JSON.
    //Endpoint: /api/GetCharacter/{id:guid}?code=<your-function-key>
    //{id:guid} has to be completely replaced by the character's id.
    [Function("GetByIdFunction")]
    public async Task GetCharacterByIdRun([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "GetCharacter/{id:guid}")] HttpRequest req, Guid id)
    {
        var response = req.HttpContext.Response;

        try
        {
            _logger.LogInformation($"Looking for character... Maybe they’re hiding behind Tobi’s mask.");

            var character = await db.GetCharacterById("Characters", id);

            if (character == null)
            {
                _logger.LogWarning("Couldn’t find the character. Did Orochimaru take them for experiments again?");
                response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            await response.WriteAsJsonAsync(character);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to fetch character. At this point just blame Danzo. {ex.Message}");
            response.StatusCode = StatusCodes.Status500InternalServerError;
            await response.WriteAsJsonAsync($"Failed to fetch character. At this point just blame Danzo. {ex.Message}");
        }
    }

    //This function uses a delete-method to delete the data of a character and returns a JSON formatted response-message.
    //Endpoint: /api/DeleteCharacter/{id:guid}?code=<your-function-key>
    //{id:guid} has to be completely replaced by the character's id.
    [Function("DeleteFunction")]
    public async Task DeleteCharacterRun([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "DeleteCharacter/{id:guid}")] HttpRequest req, Guid id)
    {
        var response = req.HttpContext.Response;
        try
        {
            _logger.LogInformation($"Deleting character.. Let's hope we don't get trapped in their genjutsu.");

            var character = db.DeleteCharacter("Characters", id);

            if (character == null)
            {
                _logger.LogWarning($"Character with Id: {id} does not exist. Are you looking for Gaara's social skills?");
                response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            await response.WriteAsJsonAsync($"Deleted character with id: {id}. Their last attempt at talk-no-jutsu failed.");
        } catch (Exception ex)
        {
            _logger.LogError($"Couldn’t delete the character. They’re clinging harder than Sasuke to his revenge plot. {ex.Message}");
            response.StatusCode = StatusCodes.Status500InternalServerError;
            await response.WriteAsJsonAsync($"Couldn’t delete the character. They’re clinging harder than Sasuke to his revenge plot. {ex.Message}");
        }
    }


    //This function uses an update-method to update an existing character's data and returns the data of the updated character formatted as JSON.
    //Endpoint: /api/UpdateCharacter/{id:guid}?code=<your-function-key>
    //{id:guid} has to be completely replaced by the character's id.
    //Request-body in Postman: { "name": "string", "village": "string", "rating": "int", "jutsu": "string" }
    //Parameters in web-browser: /api/UpdateCharacter/{id:guid}?code=<your-function-key>&name=string&village=string&rating=int&jutsu=string
    [Function("UpdateFunction")]
    public async Task UpdateCharacterRun([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "UpdateCharacter/{id:guid}")] HttpRequest req,
        Guid id, string? name, string? village, string? jutsu, int? rating)
    {
        var response = req.HttpContext.Response;
        try
        {
            _logger.LogInformation("Updating character data.. Hopefully they'll adapt better than Sasuke did to Itachi's backstory.");

            var characterToUpdate = await db.GetCharacterById("Characters", id);

            if (characterToUpdate == null)
            {
                _logger.LogWarning($"Could not find character. Are you looking for Naruto's sexy jutsu?");
                response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            if (!string.IsNullOrWhiteSpace(name) && characterToUpdate.Name != name)
            {
                characterToUpdate.Name = name;
            }

            if (!string.IsNullOrWhiteSpace(jutsu) && characterToUpdate.Jutsu != jutsu)
            {
                characterToUpdate.Jutsu = jutsu;
            }

            if (!string.IsNullOrWhiteSpace(village) && characterToUpdate.Village != village)
            {
                characterToUpdate.Village = village;
            }

            if (rating.HasValue && characterToUpdate.Rating != rating)
            {
                characterToUpdate.Rating = rating.Value;
            }

            var updatedCharacter = db.UpdateCharacterById(characterToUpdate, "Characters");

            await response.WriteAsJsonAsync(characterToUpdate);
            Console.WriteLine($"{characterToUpdate.Name}'s data is updated. Unlike Shino, we didn’t forget they exist.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to update character. Like every time Naruto tried to get Sasuke back. {ex.Message}");
            response.StatusCode = StatusCodes.Status500InternalServerError;
            await response.WriteAsJsonAsync($"Failed to update character. Like every time Naruto tried to get Sasuke back. {ex.Message}");
        }
    }
}