import Foundation

@MainActor
@Observable
class OrdersViewModel {
    
    var orders: [SimpleOrder] = []
    
    var selectedOrder: Order? = nil
    
    var statusCode: Int?
    
    func fetchOrders(orderStatus: OrderStatus) async {
        
        guard let url = URL(string: "http://localhost:5084/api/orders/status/\(orderStatus.intValue)") else {
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
                let decoder = JSONDecoder()
                let formatter = DateFormatter()
                formatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss"
                decoder.dateDecodingStrategy = .formatted(formatter)
                orders = try decoder.decode([SimpleOrder].self, from: data)
            }
            
        } catch {
            print("Failed to load orders: \(error.localizedDescription)")
            orders = []
        }
    }
    
    func fetchOrder(_ id: Int) async {
        
        guard let url = URL(string: "http://localhost:5084/api/orders/\(id)") else {
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
                let decoder = JSONDecoder()
                let formatter = DateFormatter()
                formatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss"
                decoder.dateDecodingStrategy = .formatted(formatter)
                selectedOrder = try decoder.decode(Order.self, from: data)
            }
            
        } catch {
            print("Failed to load orders: \(error.localizedDescription)")
            selectedOrder = nil
        }
    }
    
    func changeStatus(_ id: Int, _ status: Int) async {
        
        guard let url = URL(string: "http://localhost:5084/api/orders/\(id)/status?orderStatus=\(status)") else {
            print("Invalid URL for log in.")
            return
        }
        
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.setValue("Bearer \(KeychainHelper.load(forKey: "access_token") ?? "")", forHTTPHeaderField: "Authorization")
        
        do {
            let (_, response) = try await URLSession.shared.data(for: request)
            
            guard let httpResponse = response as? HTTPURLResponse else {
                print("Response was lost or has invalid format!")
                return
            }
            
            statusCode = httpResponse.statusCode
            
            if httpResponse.statusCode != 200 {
                print("\(httpResponse.statusCode) error occured")
            } else {
                print("Status changed successfully!")
            }
            
        } catch {
            print("Failed to load orders: \(error.localizedDescription)")
        }
    }
    
}
