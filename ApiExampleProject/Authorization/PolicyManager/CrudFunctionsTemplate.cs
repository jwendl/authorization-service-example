using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Attributes;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.Common.Constants;
using ApiExampleProject.Common.Interfaces;
using ApiExampleProject.Common.Pagination;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PolicyManager.DataAccess.Interfaces;
using PolicyManager.DataAccess.Models;
using PolicyManager.DataAccess.Validators;
using PolicyManager.Resources;

namespace PolicyManager
{
    public class ThingFunctions
        : BaseCrudFunctions<Thing>
    {
        public ThingFunctions(ITokenValidator tokenValidator, IJsonHttpContentValidator jsonHttpContentValidator, IDataRepository<Thing> thingRepository)
            : base(tokenValidator, jsonHttpContentValidator, thingRepository)
        {

        }

        [FunctionName(nameof(CreateThing))]
        [OpenApiOperation(nameof(CreateThing), "Create a Thing", Description = "Creates a thing item.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(Thing), Description = "The new thing object.")]
        [OpenApiResponseBody(HttpStatusCode.Created, ContentTypes.Application.Json, typeof(Thing))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> CreateThing([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "things")] HttpRequestMessage req, ILogger log)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.CreateThingStartLog);
            var httpResponseMessage = await base.CreateItemAsync<ThingValidator>(req);
            log.LogInformation(PolicyManagerCrudResources.CreateThingEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(ReadThings))]
        [OpenApiOperation(nameof(ReadThings), "Read all Things", Description = "Returns all things.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(PaginationRequest), Description = "Pagination functionality for request.")]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(IEnumerable<Thing>))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> ReadThings([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "things/{pageNumber?}/{pageSize?}")] HttpRequestMessage req, ILogger log, string pageNumber, string pageSize)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.ReadThingsStartLog);
            var httpResponseMessage = await ReadItemsAsync(req, pageNumber, pageSize);
            log.LogInformation(PolicyManagerCrudResources.ReadThingsEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(ReadThing))]
        [OpenApiOperation(nameof(ReadThing), "Read one Thing by Id", Description = "Returns an thing.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("thingId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(Thing))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> ReadThing([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "things/{thingId}")] HttpRequestMessage req, ILogger log, Guid thingId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.ReadThingStartLog);
            var httpResponseMessage = await ReadItemAsync(req, thingId);
            log.LogInformation(PolicyManagerCrudResources.ReadThingEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(UpdateThing))]
        [OpenApiOperation(nameof(UpdateThing), "Update an Thing", Description = "Updates an thing.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("environmentId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(Thing), Description = "The thing object being updated.")]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(Thing))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> UpdateThing([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "things/{thingId}")] HttpRequestMessage req, ILogger log, string thingId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.UpdateThingStartLog);
            var httpResponseMessage = await UpdateItemAsync<ThingValidator>(req, thingId);
            log.LogInformation(PolicyManagerCrudResources.UpdateThingEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(DeleteThing))]
        [OpenApiOperation(nameof(DeleteThing), "Delete an Thing", Description = "Deletes an thing.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("environmentId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.TextType.Plain, typeof(string))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> DeleteThing([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "things/{thingId}")] HttpRequestMessage req, ILogger log, string thingId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.DeleteThingStartLog);
            await DeleteItemAsync(req, thingId);
            log.LogInformation(PolicyManagerCrudResources.DeleteThingEndLog);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

namespace PolicyManager
{
    public class ThingAttributeFunctions
        : BaseCrudFunctions<ThingAttribute>
    {
        public ThingAttributeFunctions(ITokenValidator tokenValidator, IJsonHttpContentValidator jsonHttpContentValidator, IDataRepository<ThingAttribute> thingAttributeRepository)
            : base(tokenValidator, jsonHttpContentValidator, thingAttributeRepository)
        {

        }

        [FunctionName(nameof(CreateThingAttribute))]
        [OpenApiOperation(nameof(CreateThingAttribute), "Create a ThingAttribute", Description = "Creates a thingAttribute item.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(ThingAttribute), Description = "The new thingAttribute object.")]
        [OpenApiResponseBody(HttpStatusCode.Created, ContentTypes.Application.Json, typeof(ThingAttribute))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> CreateThingAttribute([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "thingAttributes")] HttpRequestMessage req, ILogger log)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.CreateThingAttributeStartLog);
            var httpResponseMessage = await base.CreateItemAsync<ThingAttributeValidator>(req);
            log.LogInformation(PolicyManagerCrudResources.CreateThingAttributeEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(ReadThingAttributes))]
        [OpenApiOperation(nameof(ReadThingAttributes), "Read all ThingAttributes", Description = "Returns all thingAttributes.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(PaginationRequest), Description = "Pagination functionality for request.")]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(IEnumerable<ThingAttribute>))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> ReadThingAttributes([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "thingAttributes/{pageNumber?}/{pageSize?}")] HttpRequestMessage req, ILogger log, string pageNumber, string pageSize)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.ReadThingAttributesStartLog);
            var httpResponseMessage = await ReadItemsAsync(req, pageNumber, pageSize);
            log.LogInformation(PolicyManagerCrudResources.ReadThingAttributesEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(ReadThingAttribute))]
        [OpenApiOperation(nameof(ReadThingAttribute), "Read one ThingAttribute by Id", Description = "Returns an thingAttribute.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("thingAttributeId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(ThingAttribute))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> ReadThingAttribute([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "thingAttributes/{thingAttributeId}")] HttpRequestMessage req, ILogger log, Guid thingAttributeId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.ReadThingAttributeStartLog);
            var httpResponseMessage = await ReadItemAsync(req, thingAttributeId);
            log.LogInformation(PolicyManagerCrudResources.ReadThingAttributeEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(UpdateThingAttribute))]
        [OpenApiOperation(nameof(UpdateThingAttribute), "Update an ThingAttribute", Description = "Updates an thingAttribute.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("environmentId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(ThingAttribute), Description = "The thingAttribute object being updated.")]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(ThingAttribute))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> UpdateThingAttribute([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "thingAttributes/{thingAttributeId}")] HttpRequestMessage req, ILogger log, string thingAttributeId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.UpdateThingAttributeStartLog);
            var httpResponseMessage = await UpdateItemAsync<ThingAttributeValidator>(req, thingAttributeId);
            log.LogInformation(PolicyManagerCrudResources.UpdateThingAttributeEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(DeleteThingAttribute))]
        [OpenApiOperation(nameof(DeleteThingAttribute), "Delete an ThingAttribute", Description = "Deletes an thingAttribute.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("environmentId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.TextType.Plain, typeof(string))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> DeleteThingAttribute([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "thingAttributes/{thingAttributeId}")] HttpRequestMessage req, ILogger log, string thingAttributeId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.DeleteThingAttributeStartLog);
            await DeleteItemAsync(req, thingAttributeId);
            log.LogInformation(PolicyManagerCrudResources.DeleteThingAttributeEndLog);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

namespace PolicyManager
{
    public class ThingPolicyFunctions
        : BaseCrudFunctions<ThingPolicy>
    {
        public ThingPolicyFunctions(ITokenValidator tokenValidator, IJsonHttpContentValidator jsonHttpContentValidator, IDataRepository<ThingPolicy> thingPolicyRepository)
            : base(tokenValidator, jsonHttpContentValidator, thingPolicyRepository)
        {

        }

        [FunctionName(nameof(CreateThingPolicy))]
        [OpenApiOperation(nameof(CreateThingPolicy), "Create a ThingPolicy", Description = "Creates a thingPolicy item.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(ThingPolicy), Description = "The new thingPolicy object.")]
        [OpenApiResponseBody(HttpStatusCode.Created, ContentTypes.Application.Json, typeof(ThingPolicy))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> CreateThingPolicy([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "thingPolicies")] HttpRequestMessage req, ILogger log)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.CreateThingPolicyStartLog);
            var httpResponseMessage = await base.CreateItemAsync<ThingPolicyValidator>(req);
            log.LogInformation(PolicyManagerCrudResources.CreateThingPolicyEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(ReadThingPolicys))]
        [OpenApiOperation(nameof(ReadThingPolicys), "Read all ThingPolicys", Description = "Returns all thingPolicys.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(PaginationRequest), Description = "Pagination functionality for request.")]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(IEnumerable<ThingPolicy>))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> ReadThingPolicys([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "thingPolicies/{pageNumber?}/{pageSize?}")] HttpRequestMessage req, ILogger log, string pageNumber, string pageSize)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.ReadThingPoliciesStartLog);
            var httpResponseMessage = await ReadItemsAsync(req, pageNumber, pageSize);
            log.LogInformation(PolicyManagerCrudResources.ReadThingPoliciesEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(ReadThingPolicy))]
        [OpenApiOperation(nameof(ReadThingPolicy), "Read one ThingPolicy by Id", Description = "Returns an thingPolicy.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("thingPolicyId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(ThingPolicy))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> ReadThingPolicy([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "thingPolicies/{thingPolicyId}")] HttpRequestMessage req, ILogger log, Guid thingPolicyId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.ReadThingPolicyStartLog);
            var httpResponseMessage = await ReadItemAsync(req, thingPolicyId);
            log.LogInformation(PolicyManagerCrudResources.ReadThingPolicyEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(UpdateThingPolicy))]
        [OpenApiOperation(nameof(UpdateThingPolicy), "Update an ThingPolicy", Description = "Updates an thingPolicy.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("environmentId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(ThingPolicy), Description = "The thingPolicy object being updated.")]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(ThingPolicy))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> UpdateThingPolicy([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "thingPolicies/{thingPolicyId}")] HttpRequestMessage req, ILogger log, string thingPolicyId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.UpdateThingPolicyStartLog);
            var httpResponseMessage = await UpdateItemAsync<ThingPolicyValidator>(req, thingPolicyId);
            log.LogInformation(PolicyManagerCrudResources.UpdateThingPolicyEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(DeleteThingPolicy))]
        [OpenApiOperation(nameof(DeleteThingPolicy), "Delete an ThingPolicy", Description = "Deletes an thingPolicy.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("environmentId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.TextType.Plain, typeof(string))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> DeleteThingPolicy([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "thingPolicies/{thingPolicyId}")] HttpRequestMessage req, ILogger log, string thingPolicyId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.DeleteThingPolicyStartLog);
            await DeleteItemAsync(req, thingPolicyId);
            log.LogInformation(PolicyManagerCrudResources.DeleteThingPolicyEndLog);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

namespace PolicyManager
{
    public class UserFunctions
        : BaseCrudFunctions<User>
    {
        public UserFunctions(ITokenValidator tokenValidator, IJsonHttpContentValidator jsonHttpContentValidator, IDataRepository<User> userRepository)
            : base(tokenValidator, jsonHttpContentValidator, userRepository)
        {

        }

        [FunctionName(nameof(CreateUser))]
        [OpenApiOperation(nameof(CreateUser), "Create a User", Description = "Creates a user item.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(User), Description = "The new user object.")]
        [OpenApiResponseBody(HttpStatusCode.Created, ContentTypes.Application.Json, typeof(User))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> CreateUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequestMessage req, ILogger log)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.CreateUserStartLog);
            var httpResponseMessage = await base.CreateItemAsync<UserValidator>(req);
            log.LogInformation(PolicyManagerCrudResources.CreateUserEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(ReadUsers))]
        [OpenApiOperation(nameof(ReadUsers), "Read all Users", Description = "Returns all users.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(PaginationRequest), Description = "Pagination functionality for request.")]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(IEnumerable<User>))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> ReadUsers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{pageNumber?}/{pageSize?}")] HttpRequestMessage req, ILogger log, string pageNumber, string pageSize)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.ReadUsersStartLog);
            var httpResponseMessage = await ReadItemsAsync(req, pageNumber, pageSize);
            log.LogInformation(PolicyManagerCrudResources.ReadUsersEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(ReadUser))]
        [OpenApiOperation(nameof(ReadUser), "Read one User by Id", Description = "Returns an user.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("userId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(User))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> ReadUser([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{userId}")] HttpRequestMessage req, ILogger log, Guid userId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.ReadUserStartLog);
            var httpResponseMessage = await ReadItemAsync(req, userId);
            log.LogInformation(PolicyManagerCrudResources.ReadUserEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(UpdateUser))]
        [OpenApiOperation(nameof(UpdateUser), "Update an User", Description = "Updates an user.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("environmentId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(User), Description = "The user object being updated.")]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(User))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> UpdateUser([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "users/{userId}")] HttpRequestMessage req, ILogger log, string userId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.UpdateUserStartLog);
            var httpResponseMessage = await UpdateItemAsync<UserValidator>(req, userId);
            log.LogInformation(PolicyManagerCrudResources.UpdateUserEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(DeleteUser))]
        [OpenApiOperation(nameof(DeleteUser), "Delete an User", Description = "Deletes an user.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("environmentId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.TextType.Plain, typeof(string))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> DeleteUser([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "users/{userId}")] HttpRequestMessage req, ILogger log, string userId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.DeleteUserStartLog);
            await DeleteItemAsync(req, userId);
            log.LogInformation(PolicyManagerCrudResources.DeleteUserEndLog);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

namespace PolicyManager
{
    public class UserAttributeFunctions
        : BaseCrudFunctions<UserAttribute>
    {
        public UserAttributeFunctions(ITokenValidator tokenValidator, IJsonHttpContentValidator jsonHttpContentValidator, IDataRepository<UserAttribute> userAttributeRepository)
            : base(tokenValidator, jsonHttpContentValidator, userAttributeRepository)
        {

        }

        [FunctionName(nameof(CreateUserAttribute))]
        [OpenApiOperation(nameof(CreateUserAttribute), "Create a UserAttribute", Description = "Creates a userAttribute item.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(UserAttribute), Description = "The new userAttribute object.")]
        [OpenApiResponseBody(HttpStatusCode.Created, ContentTypes.Application.Json, typeof(UserAttribute))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> CreateUserAttribute([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "userAttributes")] HttpRequestMessage req, ILogger log)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.CreateUserAttributeStartLog);
            var httpResponseMessage = await base.CreateItemAsync<UserAttributeValidator>(req);
            log.LogInformation(PolicyManagerCrudResources.CreateUserAttributeEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(ReadUserAttributes))]
        [OpenApiOperation(nameof(ReadUserAttributes), "Read all UserAttributes", Description = "Returns all userAttributes.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(PaginationRequest), Description = "Pagination functionality for request.")]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(IEnumerable<UserAttribute>))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> ReadUserAttributes([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "userAttributes/{pageNumber?}/{pageSize?}")] HttpRequestMessage req, ILogger log, string pageNumber, string pageSize)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.ReadUserAttributesStartLog);
            var httpResponseMessage = await ReadItemsAsync(req, pageNumber, pageSize);
            log.LogInformation(PolicyManagerCrudResources.ReadUserAttributesEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(ReadUserAttribute))]
        [OpenApiOperation(nameof(ReadUserAttribute), "Read one UserAttribute by Id", Description = "Returns an userAttribute.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("userAttributeId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(UserAttribute))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> ReadUserAttribute([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "userAttributes/{userAttributeId}")] HttpRequestMessage req, ILogger log, Guid userAttributeId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.ReadUserAttributeStartLog);
            var httpResponseMessage = await ReadItemAsync(req, userAttributeId);
            log.LogInformation(PolicyManagerCrudResources.ReadUserAttributeEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(UpdateUserAttribute))]
        [OpenApiOperation(nameof(UpdateUserAttribute), "Update an UserAttribute", Description = "Updates an userAttribute.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("environmentId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(UserAttribute), Description = "The userAttribute object being updated.")]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(UserAttribute))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> UpdateUserAttribute([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "userAttributes/{userAttributeId}")] HttpRequestMessage req, ILogger log, string userAttributeId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.UpdateUserAttributeStartLog);
            var httpResponseMessage = await UpdateItemAsync<UserAttributeValidator>(req, userAttributeId);
            log.LogInformation(PolicyManagerCrudResources.UpdateUserAttributeEndLog);
            return httpResponseMessage;
        }

        [FunctionName(nameof(DeleteUserAttribute))]
        [OpenApiOperation(nameof(DeleteUserAttribute), "Delete an UserAttribute", Description = "Deletes an userAttribute.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiParameter("environmentId", In = ParameterLocation.Path, Required = false, Type = typeof(Guid))]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.TextType.Plain, typeof(string))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> DeleteUserAttribute([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "userAttributes/{userAttributeId}")] HttpRequestMessage req, ILogger log, string userAttributeId)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerCrudResources.DeleteUserAttributeStartLog);
            await DeleteItemAsync(req, userAttributeId);
            log.LogInformation(PolicyManagerCrudResources.DeleteUserAttributeEndLog);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

