import Foundation

@MainActor
@Observable
class AgeCategoryViewModel {
    
    var ageCategories: [AgeCategory] = []
    
    var statusCode: Int?
    
    func fetchAgeCategories() async {
        
        guard let url = URL(string: "http://localhost:5084/api/age-categories") else {
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
            
            statusCode = httpResponse.statusCode
            
            if httpResponse.statusCode != 200 {
                print("\(httpResponse.statusCode) error occured")
            } else {
                ageCategories = try JSONDecoder().decode([AgeCategory].self, from: data)
            }
            
        } catch {
            print("Failed to load orders: \(error.localizedDescription)")
            ageCategories = []
        }
    }
    
}
