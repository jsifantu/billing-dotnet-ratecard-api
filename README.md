---
services: billing
platforms: dotnet
author: bryanla
---

# Microsoft Azure Billing API Code Samples: RateCard API
A simple console app that calls the Azure Billing RateCard REST API to retrieve the list of resources per Azure offer in a given subscription, along with the pricing details for each resource. The Microsoft Azure Billing APIs enable integration of Azure Billing information into your applications, providing new insights into your consumption of Azure resources, allowing you to accurately predict and manage your Azure resource consumption costs. 

The application first fetches an access token using credentials of the signed-in user, then gets the RateCard response and deserializes it and prints it to the console.  

To learn more about the Billing Usage and RateCard APIs, visit the overview article [Gain insights into your Microsoft Azure resource consumption](https://azure.microsoft.com/documentation/articles/billing-usage-rate-card-overview/).  Visit the [Azure Billing REST API Reference](https://msdn.microsoft.com/en-us/library/azure/mt218998.aspx) article for more details on each of the APIs.

## Get started

First, download the source code using your favorite **git** shell or command line (ie: Git Bash, Git for Windows, etc.). For example, if you're using Git Bash:

    git clone https://github.com/Azure-Samples/billing-dotnet-ratecard-api.git
    cd billing-dotnet-ratecard-api

Then follow the steps in [this readme](./ConsoleApp-Billing-RateCard/readme.md) to complete the required configuration work and build the sample in Visual Studio.

## Complete billing samples index
Below is the list of all of the available Azure BIlling API code samples:

-	[ConsoleApp-Billing-Usage](../ConsoleApp-Billing-Usage) - This sample will help you get started with using the Usage API.
-	[ConsoleApp-Billing-RateCard](../ConsoleApp-Billing-RateCard) - This sample help you get started with using the RateCard API.
-	[WebApp-Billing-MultiTenant](../WebApp-Billing-MultiTenant) - This Multi-Tenant sample creates a WebApp that allows the logged-in user to give it consent, to call the Azure Graph API and the Usage API on the user's behalf. It also shows the OAuth flows required to get consent for the ‘Reader’ role, to access the list of Microsoft Azure subscriptions that the user wants to allow access to. 

## Need Help?

Be sure to check out the Azure forums on [StackOverflow](http://stackoverflow.com/search?q=azure+billing) and [MSDN](https://social.msdn.microsoft.com/Forums/azure/en-US/home?forum=windowsazurepurchasing) if you are having trouble. The Azure Billing product team actively monitors the forums and will be more than happy to assist you.

If you would like to provide feedback on the Billing APIs or ideas on how we can improve them, please visit the [Azure Feedback Forums](http://feedback.azure.com/forums/170030-billing).

## Contribute Code or Provide Feedback

If you would like to become an active contributor to this project please follow the instructions provided in [Microsoft Azure Projects Contribution Guidelines](http://azure.github.com/guidelines.html).

If you encounter any bugs with the library please file an issue in the [Issues](https://github.com/Azure/BillingCodeSamples/issues) section of the repository.

## Learn More
* [Azure Billing REST API Reference ](https://msdn.microsoft.com/library/azure/1ea5b323-54bb-423d-916f-190de96c6a3c)