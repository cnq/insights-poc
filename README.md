## Synopsis

This project demonstrates how to use the Microsoft Azure SDK to create a ping web test with an alert. 

## Motivation

There was little information available online about how to programmatically create ping web tests in Azure. This code has been made available to serve as an example and proof-of-concept. It is not intended to be used as production code.

## Setup

1. In your Azure subscription's default directory, create an admin user in your domain. Note this must be a user "in your domain". It cannot be a user with a Microsoft account.
2. In the same default directory, create a new Application and take note of the Client ID. 
3. In the test class, update the constants at the top of the file with:
    *  The Azure Subscription ID
    *  The Application's Client ID created in step 2
    * The Tenant Domain of your default directory (e.g. domain.onmicrosoft.com)
    * The username of the user created in step 1 (e.g. username@domain.onmicrosoft.com)

## Contributions

This code is provided only as an example and proof-of-concept. It is not intended to become an active project. 

## License

This code is provided under the MIT license.