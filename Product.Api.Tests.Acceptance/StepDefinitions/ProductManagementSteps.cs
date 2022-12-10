using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Product.Api.Tests.Acceptance.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Product.Api.Tests.Acceptance.StepDefinitions;
//Step definition stubs can be created from the Feature file.
// mostly follow the Given When Then syntax
[Binding]
public sealed class ProductManagementSteps
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef
    //this is boiler plate but you see the point, we need a context to implement the steps here
    //it is good practice for running steps concurrently
    private readonly ScenarioContext _scenarioContext;
    //I will use a client as the context,
    //because this is registered in the Hooks Object Container, I can access here
    private readonly HttpClient _httpClient;

    public ProductManagementSteps(HttpClient httpClient, ScenarioContext scenarioContext)
    {
        _httpClient = httpClient;
        _scenarioContext = scenarioContext;
    }

    [When(@"I create products with the following details")]
    public async Task WhenICreateProductsWithTheFollowingDetails(Table table)
    {
        //create a set of CPRs from the table specified in the feature file
        var createProductRequests 
            = table.CreateSet<CreateProductRequest>();
        //have an empty set of responses to add to
        var createdProductResponses = new List<ProductResponse>();
        //iterate through the requests list
        foreach (var createProductRequest in createProductRequests)
        {
            //fire the request at the container
            var response = await _httpClient.PostAsJsonAsync("products", createProductRequest);
            var responseProduct = await response.Content.ReadFromJsonAsync<ProductResponse>();
            if (responseProduct is not null)
            {
                //add the response to the list
                createdProductResponses.Add(responseProduct);
            }
        }
        //add the list of responses to the scenario context, which gives us something to assert
        _scenarioContext.Add("CreatedProducts", createdProductResponses);
    }

    [Then(@"the products are created successfully")]
    public async Task ThenTheProductsAreCreatedSuccessfully()
    {
        var createdProducts = _scenarioContext.Get<List<ProductResponse>>("CreatedProducts");
        foreach (var createdProduct in createdProducts)
        {
            //use the id from the createdProducts in the scenario context to fire at the API
            var response = await _httpClient.GetFromJsonAsync<ProductResponse>($"products/{createdProduct.Id}");
            //check that the createdProduct is the same as the one in the db, how does this work with the db?
            createdProduct.Should().BeEquivalentTo(response);
        }
    }
    
    [Given(@"the following products in the system")]
    public async Task GivenTheFollowingProductsInTheSystem(Table table)
    {
        //create a list from the table passed down in the Given step
        var products = table.CreateSet<CreateProductRequest>();
        //create an empty list to add to the scenario context
        var createdProductsIds = new List<Guid>();
        //loop through and add them to the db and the scenario context
        foreach (var createProductRequest in products)
        {
            //fire at the create endpoint
            var response = await _httpClient.PostAsJsonAsync("products", createProductRequest);
            //extract the ProductResponse
            var responseProduct = await response.Content.ReadFromJsonAsync<ProductResponse>();
            //add to the list of ids, because delete only needs the id
            createdProductsIds.Add(responseProduct!.Id);
        }
        //add to the scenario context
        _scenarioContext.Add("CreatedProductIds", createdProductsIds);
    }

    [When(@"those products get deleted")]
    public async Task WhenThoseProductsGetDeleted()
    {
        //extract above created list from the scenario context
        var createdProductIds = _scenarioContext.Get<List<Guid>>("CreatedProductIds");
        foreach (var createdProductId in createdProductIds)
        {
            //fire them each at the delete endpoint
            await _httpClient.DeleteAsync(($"products/{createdProductId}"));
        }
    }


    [Then(@"the products are deleted successfully")]
    public async Task ThenTheProductsAreDeletedSuccessfully()
    {
        var createdProductIds = _scenarioContext.Get<List<Guid>>("CreatedProductIds");
        foreach (var createdProductId in createdProductIds)
        {
            //assert that a fire with the id at the get endpoint does not return the product
            var productResponse = await _httpClient.GetAsync(($"products/{createdProductId}"));
            //depending on the type of ACP tests, this could be a call to the db itself
            //ie look in the data context, or in Hansen, check that a file has been created, 
            //or the embeddeddataset has the right data set in it for example
            productResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}