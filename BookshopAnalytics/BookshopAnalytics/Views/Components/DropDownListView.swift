//
//  DropDownListView.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 10/08/2025.
//

import SwiftUI

struct DropDownListView: View {
    
    var title: String
    
    @State var model = AgeCategoryViewModel()
    
    @Binding var selectedId: Int?
    @Binding var error: ErrorType
    
    
    var body: some View {
        VStack(alignment: .leading) {
            Text(title)
                .fontWeight(.medium)
                .font(.footnote)
            Picker("", selection: $selectedId) {
                ForEach(model.ageCategories) { ageCategory in
                    Text("\(ageCategory.tag) (\(ageCategory.minimumAge)+)").tag(Optional(ageCategory.id))
                }
            }.padding(.leading, -8)
            if error != .correct {
                Text("􀁟 \(error.rawValue) \(title.lowercased())!")
                    .foregroundStyle(.red)
                    .font(.footnote)
            }
        }
        .task {
            await model.fetchAgeCategories()
        }
    }
}

#Preview {
    DropDownListView(title: "Age Category", selectedId: .constant(nil), error: .constant(.zeroChoiceOne))
}
