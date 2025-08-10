import Foundation

@MainActor
@Observable
class LogInViewModel {
    
    var userTokens: UserTokens?
    
    var statusCode: Int?
    
    func logIn(logInCredentials: LogInCredentials) async {

        guard let url = URL(string: "http://localhost:5084/api/auth/employees/login") else {
            print("Invalid URL for log in.")
            return
        }
        
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
            print("Error \(error.localizedDescription)")
        }
    }
    
    func refresh(refreshCredentials: UserTokens) async {

        guard let url = URL(string: "http://localhost:5084/api/auth/refresh") else {
            print("Invalid URL for log in.")
            return
        }
        
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")

        do {
            
            let refreshCredentialsJson = try JSONEncoder().encode(refreshCredentials)
            request.httpBody = refreshCredentialsJson
            
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
            print("Error \(error.localizedDescription)")
        }
    }
    
}


