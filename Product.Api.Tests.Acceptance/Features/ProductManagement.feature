Feature: Manage products in the system

    Scenario: Products get created successfully
        When I create products with the following details
        #use of table here allows us to pass through multiple objects
          | Name    | Description         | Price | RRP   | CurrencyIsoCode | Category |
          | Hoodie  | Keep coding hoodie  | 20.99 | 20.99 | GBP             | Clothing |
          | T-Shirt | Keep coding t-shirt | 18.99 | 18.99 | GBP             | Clothing |
        Then the products are created successfully

    Scenario: Products get deleted successfully
        Given the following products in the system
          | Name    | Description         | Price | RRP   | CurrencyIsoCode | Category |
          | Hoodie  | Keep coding hoodie  | 20.99 | 20.99 | GBP             | Clothing |
          | T-Shirt | Keep coding t-shirt | 18.99 | 18.99 | GBP             | Clothing |
        When those products get deleted
        Then the products are deleted successfully
    