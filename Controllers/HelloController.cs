using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _4kTiles_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        /// <summary>
        /// Initial api call for get method
        /// </summary>
        /// <returns>Hello world</returns>
        [HttpGet]
        public string PrintHello()
        {
            return "Hello World";
        }

        /// <summary>
        /// Initial api call for post method
        /// </summary>
        /// <param name="name">name to post</param>
        /// <returns>Hello with provided name</returns>
        [HttpPost]
        public string PostHello(string name)
        {
            return $"Hello {name}";
        }

        /// <summary>
        /// Initial api call for patch method
        /// </summary>
        /// <param name="people">people to patch</param>
        /// <returns>people</returns>
        [HttpPatch]
        public People PatchHello(People people)
        {
            return new People
            {
                Name = people.GetNameReverse(),
            };
        }
    }

    public class People
    {
        private string _name;

        public People() { }

        public string Name { get; set; }

        public string GetNameReverse()
        {
            return Reverse(Name);
        }

        private string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
