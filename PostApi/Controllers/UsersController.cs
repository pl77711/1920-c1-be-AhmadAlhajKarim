﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostApi.DTO;
using PostApi.Models;
using RecipeApi.Models;

namespace PostApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository context)
        {
            _userRepository = context;
        }

        // GET: api/Users
        /// <summary>
        /// Get all users ordered by name
        /// </summary>
        /// <returns>array of users</returns>
        [HttpGet]
        public IEnumerable<User> GetUsers(string name = null)
        {
            if (string.IsNullOrEmpty(name))
                return _userRepository.GetAll();
            return _userRepository.GetBy(name);
        }

        // GET: api/Users/5
        /// <summary>
        /// Get the user with given id
        /// </summary>
        /// <param name="id">the id of the user</param>
        /// <returns>The user</returns>
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            User user= _userRepository.GetBy(id);
            if (user == null) return NotFound();
            return user;
        }

        /*[HttpPost]
        public ActionResult<User> PostUser(UserDTO user)
        {
            User newUser = new User() { Name = user.Name, Password = user.Password };
            foreach (var i in recipe.Ingredients)
                newUser.AddPost(new Post(i.Title, i.Amount, i.Unit));
            _recipeRepository.Add(recipeToCreate);
            _recipeRepository.SaveChanges();

            return CreatedAtAction(nameof(GetRecipe), new { id = recipeToCreate.Id }, recipeToCreate);
        }*/

        // PUT: api/Users/5
        /// <summary>
        /// Modifies a user
        /// </summary>
        /// <param name="id">id of the user to be modified</param>
        /// <param name="recipe">the modified user</param>
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            _userRepository.Update(user);
            _userRepository.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Recipes/5
        /// <summary>
        /// Deletes a recipe
        /// </summary>
        /// <param name="id">the id of the recipe to be deleted</param>

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            User recipe = _userRepository.GetBy(id);
            if (recipe == null)
            {
                return NotFound();
            }
            _userRepository.Delete(recipe);
            _userRepository.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Get an post for a user
        /// </summary>
        /// <param name="id">id of the user</param>
        /// <param name="ingredientId">id of the post</param>
        [HttpGet("{id}/ingredients/{ingredientId}")]
        public ActionResult<Post> GetPost(int id, int postId)
        {
            if (!_userRepository.TryGetUser(id, out var user))
            {
                return NotFound();
            }
            Post post = user.GetPost(postId);
            if (post == null)
                return NotFound();
            return post;
        }

        /// <summary>
        /// Adds a post to a user
        /// </summary>
        /// <param name="id">the id of the user</param>
        /// <param name="ingredient">the post to be added</param>
        [HttpPost("{id}/ingredients")]
        public ActionResult<Post> PostIngredient(int id, PostDTO post)
        {
            if (!_userRepository.TryGetUser(id, out var user))
            {
                return NotFound();
            }
            var newPost = new Post(post.Title, post.Location, post.PictureUrl);
            user.AddPost(newPost);
            _userRepository.SaveChanges();
            return CreatedAtAction("GetPost", new { id = user.Id, postId = newPost.Id }, newPost);
        }
    }
}