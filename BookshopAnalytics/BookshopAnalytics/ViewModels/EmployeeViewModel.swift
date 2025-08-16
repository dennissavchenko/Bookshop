import Foundation

@MainActor
@Observable
class EmployeeViewModel {
    
    var employee: Employee?
    
    var statusCode: Int?
    
    func fetchEmployee(employeeId: Int, wasRefreshed: Bool = false) async -> Int {

        guard let url = URL(string: "http://localhost:5084/api/users/employees/\(employeeId)") else {
            print("Invalid URL for log in.")
            return
        }
        
        var request = URLRequest(url: url)
        request.httpMethod = "GET"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.setValue("Bearer \(KeychainHelper.load(forKey: "access_token") ?? "")", forHTTPHeaderField: "Authorization")

        do {
            let (data, response) = try await URLSession.shared.data(for: request)
            
            guard let httpResponse = response as? HTTPURLResponse else {
                print("Response was lost or has invalid format!")
                return
            }
            
            if httpResponse.statusCode == 401 && !wasRefreshed {
                let refreshed = await LogInViewModel().refresh(
                    refreshCredentials: UserTokens(
                        id: 0,
                        accessToken: KeychainHelper.load(forKey: "access_token") ?? "",
                        refreshToken: KeychainHelper.load(forKey: "refresh_token") ?? ""
                    )
                )
                if refreshed {
                    return await fetchEmployee(employeeId: employeeId, wasRefreshed: true)
                } else {
                    print("Token refresh failed")
                    return 401
                }
            }
            
            if httpResponse.statusCode != 200 {
                print("\(httpResponse.statusCode) error occured")
            } else {
                employee = try JSONDecoder().decode(Employee.self, from: data)
            }
            
        } catch {
            print("Error \(error.localizedDescription)")
        }
    }
}
