### Setup Instructions

1. **Install .NET SDK 8.0**: Ensure you have the .NET SDK 8.0 installed. You can download it from the [official .NET website](https://dotnet.microsoft.com/download).

2. **Clone the Repository**:
    ```bash
    git clone https://github.com/denisams/CircuitBreakerDemo.git
    cd CircuitBreakerDemo
    ```

3. **Add Polly Packages**:
    ```bash
    dotnet add package Polly
    dotnet add package Polly.Extensions.Http
    ```

4. **Restore Packages**:
    ```bash
    dotnet restore
    ```

5. **Run the Project**:
    ```bash
    dotnet run
    ```

### Testing the Service

1. **Start the Application**: Once the project is running, the application will be available at `http://localhost:7149`.

2. **Test the Endpoint**:
    - Use a web browser or a tool like Postman to send a GET request to `http://localhost:7149/test`.

### Expected Behavior

1. **Successful Response**:
    - When the external endpoint is available, you should receive a 200 OK status with the JSON content.

2. **Triggering the Circuit Breaker**:
    - If the external endpoint fails, after two consecutive failures, the Circuit Breaker will open.
    - Subsequent requests will return a 503 Service Unavailable status until the Circuit Breaker resets.

This project demonstrates how to implement the Circuit Breaker pattern in a .NET 8.0 application using Polly to enhance resilience and fault tolerance.
