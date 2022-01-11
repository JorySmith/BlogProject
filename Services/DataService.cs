﻿using BlogProject.Data;
using BlogProject.Enums;
using BlogProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Services
{
    public class DataService
    {
        // Add private readonly properties to store _dbContext, _roleManager, _userManager
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        // Constructor injection into every instance of DataService, ApplicationDbContext and RoleManager
        public DataService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // 
        public async Task ManageDataAsync()
        {
            // Create DB from migrations using MigrateAsync
            await _dbContext.Database.MigrateAsync();
            
            // Seed a few user roles
            await SeedRolesAsync();

            // Seed users/create new users
            await SeedUsersAsync();

        }

        // Seed roles private method
        private async Task SeedRolesAsync()
        {
            // If there are roles in the DB, do nothing
            if(_dbContext.Roles.Any())
            {
                return;
            }
            // Otherwise, create a few roles from Enum BlogRole
            foreach (var role in Enum.GetNames(typeof(BlogRole)))
            {
                // Use Role Manager to create roles
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed users private method
        private async Task SeedUsersAsync()
        {
            // If there are users in the DB, do nothing
            if (_dbContext.Users.Any())
            {
                return;
            }

            // Otherwise, create a new adminUser instance 
            var adminUser = new BlogUser()
            {
                Email = "Jory320@gmail.com",
                UserName = "Jory320@gmail.com",
                FirstName = "Jory",
                LastName = "Smith",
                PhoneNumber = "(800) 555-1212",
                EmailConfirmed = true
            };
            // Use UserManager to create an adminUser
            await _userManager.CreateAsync(adminUser, "asdf1234");

            // Assign BlogRole.Administrator to adminUser, ensure role is a string
            await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());
            
            // Create a new modUser instance 
            var modUser = new BlogUser()
            {
                Email = "Jory320@gmail.com",
                UserName = "Jory320@gmail.com",
                FirstName = "Jory",
                LastName = "Smith",
                PhoneNumber = "(800) 555-1212",
                EmailConfirmed = true
            };
            // Use UserManager to create a modUser
            await _userManager.CreateAsync(modUser, "asdf1234");

            // Assign BlogRole.Moderator to modUser, ensure role is a string
            await _userManager.AddToRoleAsync(modUser, BlogRole.Moderator.ToString());
        }
    }
}
