﻿global using AutoMapper;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ApiExplorer;
global using Microsoft.AspNetCore.Mvc.Versioning;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Any;
global using Microsoft.OpenApi.Models;
global using Newtonsoft.Json;
global using NLog.Web;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using System;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.IdentityModel.Tokens.Jwt;
global using System.IO;
global using System.Linq;
global using System.Net.Mime;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text;
global using System.Text.Json;
global using System.Threading.Tasks;
global using SportStore.Data;
global using SportStore.Filters;
global using SportStore.Helpers;
global using SportStore.Managers;
global using SportStore.Models;
global using SportStore.Extensions;
global using SportStore.Managers.Repositories;
global using SportStore.Models.Dtos;
global using SportStore.Models.Entities;
global using SportStore.Models.Results;

/////ADDED
global using Microsoft.AspNetCore.JsonPatch;
global using Swashbuckle.AspNetCore.Annotations;
global using Microsoft.AspNetCore.Hosting;
global using System.Linq.Expressions;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Mvc.ModelBinding;

