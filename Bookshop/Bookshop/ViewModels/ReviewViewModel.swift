//
//  OrderViewModel.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation

/// A view model responsible for handling operations related to customer reviews,
/// such as submitting a new review to the backend.
///
/// This class operates on the `@MainActor` to ensure any UI-related logic
/// triggered by its methods (though not directly present here) runs on the main thread.
@MainActor
class ReviewViewModel {

    /// Submits a new review to the API.
    ///
    /// This asynchronous function encodes the `SendReview` object and sends it as
    /// a POST request to the review endpoint.
    /// - Parameter review: The `SendReview` object containing the review details.
    /// - Returns: `true` if the review was successfully submitted (HTTP status 201),
    ///            `false` otherwise (e.g., invalid URL, network error, or non-201 status).
    func submitReview(_ review: SendReview) async -> Bool {
        // Construct the URL for the review submission endpoint.
        guard let url = URL(string: "http://localhost:5084/api/reviews") else {
            print("Invalid URL for review submission.")
            return false
        }

        // Configure the URL request for a POST operation.
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")

        do {
            // Encode the SendReview object to JSON data and set it as the request body.
            let jsonData = try JSONEncoder().encode(review)
            request.httpBody = jsonData

            // Perform the network request.
            let (_, response) = try await URLSession.shared.data(for: request)

            // Check the HTTP response status code.
            if let httpResponse = response as? HTTPURLResponse {
                // Return true if the status code is 201 (Created).
                return httpResponse.statusCode == 201
            } else {
                // If the response is not an HTTPURLResponse, consider it a failure.
                print("Invalid HTTP response received.")
                return false
            }
        } catch {
            // Catch and print any errors during encoding or network request, then return false.
            print("Error submitting review:", error)
            return false
        }
    }
}
