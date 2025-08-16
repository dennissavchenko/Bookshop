import Foundation

@MainActor
@Observable
class ItemsViewModel {
    
    var items: [SimpleItem] = []
    
    var selectedItem: Item? = nil
    var selectedAddItem: AddItem? = nil
    
    var statusCode: Int?
    
    func fetchItems() async {

        guard let url = URL(string: "http://localhost:5084/api/items") else {
            print("Invalid URL for log in.")
            return
        }
        
        var request = URLRequest(url: url)
        request.httpMethod = "GET"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")

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
                items = try JSONDecoder().decode([SimpleItem].self, from: data)
            }
            
        } catch {
            print("Failed to load employee: \(error.localizedDescription)")
            items = []
        }
    }
    
    func deleteItem(_ id: Int, wasRefreshed: Bool = false) async -> Int {

        guard let url = URL(string: "http://localhost:5084/api/items/\(id)") else {
            print("Invalid URL for log in.")
            return 0
        }
        
        var request = URLRequest(url: url)
        request.httpMethod = "DELETE"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.setValue("Bearer \(KeychainHelper.load(forKey: "access_token") ?? "")", forHTTPHeaderField: "Authorization")

        do {
            let (_, response) = try await URLSession.shared.data(for: request)
            
            guard let httpResponse = response as? HTTPURLResponse else {
                print("Response was lost or has invalid format!")
                return 0
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
                    return await deleteItem(id, wasRefreshed: true)
                } else {
                    print("Token refresh failed")
                    return 401
                }
            }
            
            if httpResponse.statusCode != 204 {
                print("\(httpResponse.statusCode) error occured")
            } else {
                print("Deleted successfully!")
                items.removeAll(where: { $0.id == id })
            }
            
            return httpResponse.statusCode
            
        } catch {
            print("Failed to load employee: \(error.localizedDescription)")
            return 0
        }
    }
    
    func fetchItem(_ id: Int) async {

        guard let url = URL(string: "http://localhost:5084/api/items/\(id)") else {
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
                selectedItem = try decoder.decode(Item.self, from: data)
            }
            
        } catch {
            print("Failed to load employee: \(error.localizedDescription)")
        }
    }
    
    func getAddItem(_ id: Int) async {
        await fetchItem(id)
        if let item = selectedItem {
            selectedAddItem = AddItem(id: item.id, name: item.name, description: item.description, imageUrl: item.imageUrl, publishingDate: item.publishingDate, language: item.language, price: String(format: "%.2f", item.price), amountInStock: String(item.amountInStock), publisherId: item.publisherId, ageCategoryId: item.ageCategoryId, itemType: item.type, authorsIds: item.authorsIds ?? [], genresIds: item.genresIds ?? [], numberOfPages: String(item.numberOfPages ?? 0), coverType: item.coverType, isSpecialEdition: item.isSpecialEdition ?? false, headline: item.headline ?? "", topics: item.topics ?? [], itemCondition: item.isUsed ? .used : .new, isSealed: item.isSealed ?? false, condition: item.condition, hasAnnotations: item.hasAnnotations ?? false)
        }
    }
    
    func addItem(item: AddItem, wasRefreshed: Bool = false) async -> Int {
        
        guard let url = URL(string: "http://localhost:5084/api/items/\(item.itemCondition?.rawValue.lowercased() ?? "new")\(item.itemType == .typeless ? "" : "/\(item.itemType?.rawValue.lowercased() ?? "book")")") else {
            print("Invalid URL for log in.")
            return 0
        }
        
        print(url.absoluteString)
        
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.setValue("Bearer \(KeychainHelper.load(forKey: "access_token") ?? "")", forHTTPHeaderField: "Authorization")
        
        do {
            
            let jsonData = try JSONSerialization.data(withJSONObject: getItemJSON(item), options: .prettyPrinted)
            
            request.httpBody = jsonData

            let (_, response) = try await URLSession.shared.data(for: request)
            
            guard let httpResponse = response as? HTTPURLResponse else {
                print("Response was lost or has invalid format!")
                return 0
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
                    return await addItem(item: item, wasRefreshed: true)
                } else {
                    print("Token refresh failed")
                    return 401
                }
            }
            
            if httpResponse.statusCode != 201 {
                print("\(httpResponse.statusCode) error occured")
            } else {
                print("Created Successfully")
            }
            
            return httpResponse.statusCode
            
        } catch {
            print("Error!")
            return 0
        }
    }
    
    func updateItem(item: AddItem, wasRefreshed: Bool = false) async -> Int {
        guard let url = URL(string:
            "http://localhost:5084/api/items/\(item.itemCondition?.rawValue.lowercased() ?? "new")" +
            "\(item.itemType == .typeless ? "" : "/\(item.itemType?.rawValue.lowercased() ?? "book")")"
        ) else {
            print("Invalid URL for update.")
            return 0
        }

        var request = URLRequest(url: url)
        request.httpMethod = "PUT"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.setValue("Bearer \(KeychainHelper.load(forKey: "access_token") ?? "")",
                         forHTTPHeaderField: "Authorization")

        do {
            let jsonData = try JSONSerialization.data(withJSONObject: getItemJSON(item), options: [])
            request.httpBody = jsonData

            let (_, response) = try await URLSession.shared.data(for: request)
            guard let httpResponse = response as? HTTPURLResponse else {
                print("Response was lost or has invalid format!")
                return 0
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
                    return await updateItem(item: item, wasRefreshed: true)
                } else {
                    print("Token refresh failed")
                    return 401
                }
            }

            if httpResponse.statusCode != 204 {
                print("\(httpResponse.statusCode) error occurred \(wasRefreshed)")
            } else {
                print("Updated Successfully \(wasRefreshed)")
            }

            return httpResponse.statusCode

        } catch {
            print("Error! \(error.localizedDescription)")
            return 0
        }
    }

    
    private func getUsedJSON(_ item: AddItem) -> [String: Any] {
        [
            "condition": item.condition?.intValue ?? 0,
            "hasAnnotations": item.hasAnnotations
        ]
    }
    
    private func getNewJSON(_ item: AddItem) -> [String: Any] {
        ["isSealed": item.isSealed]
    }
    
    private func getBookJSON(_ item: AddItem) -> [String: Any] {
        [
            "numberOfPages": item.numberOfPages,
            "coverType": item.coverType?.intValue ?? 0,
            "authorsIds": item.authorsIds,
            "genresIds": item.genresIds
        ]
    }
    
    private func getMagazineJSON(_ item: AddItem) -> [String: Any] {
        ["isSpecialEdition": item.isSpecialEdition]
    }
    
    private func getNewspaperJSON(_ item: AddItem) -> [String: Any] {
        [
            "headline": item.headline,
            "topics": item.topics
        ]
    }
    
    private func getItemJSON(_ item: AddItem) -> [String: Any] {
        var json: [String: Any] = [
            "id": item.id,
            "name": item.name,
            "description": item.description,
            "imageUrl": item.imageUrl,
            "publishingDate": item.publishingDate.customFormatDateFormatter(format: "yyyy-MM-dd'T'HH:mm:ss"),
            "language": item.language ?? "",
            "price": item.price,
            "amountInStock": item.amountInStock,
            "publisherId": item.publisherId ?? 0,
            "ageCategoryId": item.ageCategoryId ?? 0
        ]
        
        let conditionKey = item.itemCondition == .new ? "new" : "used"
        json[conditionKey] = item.itemCondition == .new ? getNewJSON(item) : getUsedJSON(item)
        
        if item.itemType != .typeless {
            switch item.itemType {
            case .book:
                json["book"] = getBookJSON(item)
            case .magazine:
                json["magazine"] = getMagazineJSON(item)
            case .newspaper:
                json["newspaper"] = getNewspaperJSON(item)
            default:
                break
            }
        }
        
        return json
    }
 
}
