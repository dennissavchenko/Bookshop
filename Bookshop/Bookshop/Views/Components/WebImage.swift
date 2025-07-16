//
//  WebImage.swift
//  Books
//
//  Created by dennis savchenko on 22/08/2024.
//

import SwiftUI

struct WebImage: View {
    
    let imageURL: String
    @Binding var isLoaded: Bool
        
    var body: some View {
        
        AsyncImage(url: URL(string: imageURL)) { phase in
            switch phase {
            case .empty:
                ProgressView()
                    .onAppear {
                        isLoaded = false
                    }
            case .success(let image):
                image
                    .resizable()
                    .onAppear {
                        isLoaded = true
                    }
            case .failure:
                ProgressView()
                    .onAppear {
                        isLoaded = false
                    }
            @unknown default:
                EmptyView()
                    .onAppear {
                        isLoaded = false
                    }
            }
        }
    }
}

#Preview {
    WebImage(imageURL: "https://m.media-amazon.com/images/I/71CX5LhK9UL._AC_UF1000,1000_QL80_.jpg", isLoaded: .constant(false))
}
