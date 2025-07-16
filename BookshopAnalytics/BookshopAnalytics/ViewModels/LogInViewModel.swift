//
//  OrderViewModel.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation

/// A view model responsible for logging user in and returning its tokens.
///
/// This class is an `@Observable` object, making it suitable for use in SwiftUI views
/// to automatically update the UI when item data or loading states change.
/// It operates on the `@MainActor` to ensure UI updates happen on the main thread.
@MainActor
@Observable
class LogInViewModel {
    
    /// The userTokens object, loaded from the API.
    var userTokens: UserTokens?
    
    /// Status code of the response
    var statusCode: Int?
    
    /// Loads the tokes for a specific user from the API.
    ///
    /// - Parameter logInCredentials: Sturcture that contains username or email and password attributes
    func logIn(logInCredentials: LogInCredentials) async {

        guard let url = URL(string: "http://localhost:5084/api/auth/employees/login") else {
            print("Invalid URL for log in.")
            return
        }
        
        // Configure the URL request for a POST operation.
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")

        do {
            
            let logInCredentialsJson = try JSONEncoder().encode(logInCredentials)
            request.httpBody = logInCredentialsJson
            
            let (data, response) = try await URLSession.shared.data(for: request)
            
            guard let httpResponse = response as? HTTPURLResponse else {
                print("Response was lost or has invalid format!")
                return
            }
            
            statusCode = httpResponse.statusCode
            
            if httpResponse.statusCode != 200 {
                print("\(httpResponse.statusCode) error occured")
            } else {
                userTokens = try JSONDecoder().decode(UserTokens.self, from: data)
                if let userTokens = userTokens {
                    KeychainHelper.save(String(userTokens.id), forKey: "user_id")
                    KeychainHelper.save(userTokens.accessToken, forKey: "access_token")
                    KeychainHelper.save(userTokens.refreshToken, forKey: "refresh_token")
                }
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
                print("Failed to load user tokens: \(error.localizedDescription)")
                userTokens = nil
            }
        }
    }
}
