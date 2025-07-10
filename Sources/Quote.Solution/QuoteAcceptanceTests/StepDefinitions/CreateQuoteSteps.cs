// ----------------------------------------------------------------------
// <copyright file="IsAliveSteps.cs">
//  Copyright 2024
// </copyright>
// ----------------------------------------------------------------------

namespace QuoteAcceptanceTests.StepDefinitions
{
    using System;
    using Models;
    using Reqnroll;
    using System.Net;
    using Newtonsoft.Json;
    using System.Text;


    // <summary>
    /// The is alive steps.
    /// </summary>
    [Binding, Scope(Tag = "CreateQuote")]
    internal class CreateQuoteSteps
    {

        private HttpClient client = new HttpClient();
        private List<dynamic> quoteRequests = new List<dynamic>();
        private List<HttpResponseMessage> responses = new List<HttpResponseMessage>();



        #region Given


        [Given(@"the following quote details:")]
        public void GivenTheFollowingQuoteDetails(Table table)
        {
            foreach (var row in table.Rows)
            {
                quoteRequests.Add(new
                {
                    customer = row["customer"],
                    items = new[]
                    {
                        new {
                            item = row["item"],
                            quantity = int.Parse(row["quantity"]),
                            unitaryPrice = decimal.Parse(row["price"]),
                            discountPercentage = 0 // or set as needed
                         }
                    }
                });
            }
        }


        #endregion

        #region When

        [When(@"I create quotes for those customers")]
        public void WhenICreateQuotesForThoseCustomers()
        {
            var client = new HttpClient();
            responses.Clear();
            foreach (var request in quoteRequests)
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var uri = new Uri("https://localhost:59251/api/Quotes/create");
                var response = client.PostAsync(uri, content).Result;
                responses.Add(response);
            }
        }
        #endregion

        #region Then


        [Then(@"it returns quotes with the correct details and a confirmation message ""(.*)""")]
        public void ThenItReturnsQuotesWithTheCorrectDetailsAndAConfirmationMessage(string expectedMessage)
        {
            for (int i = 0; i < responses.Count; i++)
            {
                var response = responses[i];
                Assert.IsNotNull(response, "No response received from API.");
                Assert.IsNotNull(response.Content, "No response content received from API.");
                var responseBody = response.Content.ReadAsStringAsync().Result;
                dynamic? result = JsonConvert.DeserializeObject(responseBody);
                Assert.IsNotNull(result, "Deserialized response is null.");

                // Check confirmation message
                Assert.IsNotNull(result.confirmation, "Confirmation object is null.");
                Assert.AreEqual(expectedMessage, (string)result.confirmation.message);

                // Check quote details
                var expected = quoteRequests[i];
                Assert.AreEqual(expected.customer, (string)result.quote.customer);
                Assert.AreEqual(expected.items[0].item, (string)result.quote.lines[0].item);
                Assert.AreEqual(expected.items[0].quantity, (int)result.quote.lines[0].quantity);
                Assert.AreEqual(expected.items[0].unitaryPrice, (decimal)result.quote.lines[0].unitaryPrice);

                // Handle discount if present
                decimal unitaryPrice = expected.items[0].unitaryPrice;
                int quantity = expected.items[0].quantity;
                decimal discount = 0;
                var dict = expected.items[0] as IDictionary<string, object>;
                if (dict != null && dict.ContainsKey("discount"))
                    discount = Convert.ToDecimal(dict["discount"]);

                decimal expectedDiscountAmount = unitaryPrice * quantity * discount / 100;
                decimal expectedLinePrice = unitaryPrice * quantity - expectedDiscountAmount;

                // If discount is present, check discountAmount; otherwise, skip
                if (discount > 0)
                    Assert.AreEqual(expectedDiscountAmount, (decimal)result.quote.lines[0].discountAmount);

                Assert.AreEqual(expectedLinePrice, (decimal)result.quote.lines[0].linePrice);
            }
        }

        [Then(@"it returns an error message ""(.*)""")]
        public void ThenItReturnsAnErrorMessage(string expectedErrorMessage)
        {
            // Assume only one response for error scenarios
            Assert.AreEqual(1, responses.Count, "Expected exactly one response for error scenario.");
            var response = responses[0];
            Assert.IsNotNull(response, "No response received from API.");
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "Expected 400 Bad Request.");

            var responseBody = response.Content?.ReadAsStringAsync().Result;
            Assert.IsNotNull(responseBody, "No response body received from API.");

            // The API may return a plain string or a JSON object with a message
            // Try to parse as JSON first, fallback to plain string
            try
            {
                dynamic? result = JsonConvert.DeserializeObject(responseBody);
                if (result != null && result.message != null)
                {
                    Assert.AreEqual(expectedErrorMessage, (string)result.message);
                }
                else
                {
                    Assert.AreEqual(expectedErrorMessage, responseBody.Trim('"'));
                }
            }
            catch
            {
                // Not JSON, compare as plain string
                Assert.AreEqual(expectedErrorMessage, responseBody.Trim('"'));
            }
        }

        [Then(@"it returns a confirmation message ""(.*)"" and no quote is returned")]
        public void ThenItReturnsAConfirmationMessageAndNoQuoteIsReturned(string expectedMessage)
        {
            Assert.AreEqual(1, responses.Count, "Expected exactly one response for this scenario.");
            var response = responses[0];
            Assert.IsNotNull(response, "No response received from API.");
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode, "Expected 200 OK.");

            var responseBody = response.Content?.ReadAsStringAsync().Result;
            Assert.IsNotNull(responseBody, "No response body received from API.");

            dynamic? result = JsonConvert.DeserializeObject(responseBody);
            Assert.IsNotNull(result, "Deserialized response is null.");

            Assert.IsNull(result.quote, "Quote should be null.");
            Assert.IsNotNull(result.confirmation, "Confirmation object is null.");
            Assert.AreEqual(expectedMessage, (string)result.confirmation.message);
        }
        
        [Then(@"it returns a validation error containing ""(.*)""")]
        public void ThenItReturnsAValidationErrorContaining(string expectedErrorSubstring)
        {
            Assert.AreEqual(1, responses.Count, "Expected exactly one response for error scenario.");
            var response = responses[0];
            Assert.IsNotNull(response, "No response received from API.");
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode, "Expected 400 Bad Request.");

            var responseBody = response.Content?.ReadAsStringAsync().Result;
            Assert.IsNotNull(responseBody, "No response body received from API.");

            // Check if the expected error substring is present in the response body
            Assert.IsTrue(responseBody.Contains(expectedErrorSubstring),
                $"Expected error message to contain '{expectedErrorSubstring}', but got: {responseBody}");
        }
        #endregion

    }
}
