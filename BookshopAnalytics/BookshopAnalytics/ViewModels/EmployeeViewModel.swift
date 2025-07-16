//
//  OrderViewModel.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation

/// A view model responsible for managing employees' communications with the server
///
/// This class is an `@Observable` object, making it suitable for use in SwiftUI views
/// to automatically update the UI when item data or loading states change.
/// It operates on the `@MainActor` to ensure UI updates happen on the main thread.
@MainActor
@Observable
class EmployeeViewModel {
    
    /// The userTokens object, loaded from the API
    var employee: Employee?
    
    /// Status code of the response
    var statusCode: Int?
    
    /// Loads the data for a specific employee from the API.
    ///
    /// - Parameter employeeId: Id of an employee to fetch
    func fetchEmployee(employeeId: Int) async {

        guard let url = URL(string: "http://localhost:5084/api/users/employees/\(employeeId)") else {
            print("Invalid URL for log in.")
            return
        }
        
        // Configure the URL request for a POST operation.
        var request = URLRequest(url: url)
        request.httpMethod = "GET"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.setValue("Bearer \(KeychainHelper.load(forKey: "access_token") ?? "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkYXZpZC5sZWVAZXhhbXBsZS5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJFbXBsb3llZSIsImp0aSI6ImNhYzk4MWE2LWI4NmMtNGZlMi1iYTVlLWNjYmYzYzBhMzQzOCIsImV4cCI6MTc1MDg3MjE3MSwiaXNzIjoiQm9va1Nob3BTZXJ2ZXIiLCJhdWQiOiJCb29rU2hvcCJ9.ImTc3JwCjYvI4ShY879pbWrMsqTKXty5qQfPOP6lyuo")", forHTTPHeaderField: "Authorization")

        do {
            let (data, response) = try await URLSession.shared.data(for: request)
            
            guard let httpResponse = response as? HTTPURLResponse else {
                print("Response was lost or has invalid format!")
                return
            }
            
            statusCode = httpResponse.statusCode
            
            if httpResponse.statusCode != 200 {
                print("\(httpResponse.statusCode) error occured")
            } else {
                employee = try JSONDecoder().decode(Employee.self, from: data)
            }
            
        } catch {
            // Handle NSURLSession cancellation
            if let urlErr = error as? URLError, urlErr.code == .cancelled {
                // benign cancellation: do not reset item or show error
            }
            else if Task.isCancelled {
                // Swift concurrency cancellation: ignore
            }
            else {
                print("Failed to load employee: \(error.localizedDescription)")
                employee = nil
            }
        }
    }
}
