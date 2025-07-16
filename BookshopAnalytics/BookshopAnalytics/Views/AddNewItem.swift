//
//  AddNewItem.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 06/07/2025.
//

import SwiftUI

struct AddNewItem: View {
    
    @State var item = AddItem(id: 0, name: "", description: "", imageUrl: "", publishingDate: Date(), language: nil, price: "", amountInStock: "", publisherId: nil, ageCategoryId: nil, itemType: ItemType.book, authorsIds: [], genresIds: [], numberOfPages: "", coverType: nil, isSpecialEdition: nil, headline: "", topics: nil, isUsed: false, isSealed: nil, condition: nil, hasAnnotations: nil)
    
    @State var isLoaded = false
    
    @State private var addButtonPressed = false
    
    var body: some View {
        ScrollView {
            VStack(alignment: .leading, spacing: 8) {
                getTextField("Title", "ex. Left Neglected", $item.name)
                getTextField("Description", "ex. Left Neglected tells the story of Sarah Nickerson, a driven career woman and mother...", $item.description)
                getTextField("Image URL", "ex. https://amazon.com/images/left_neglected.jpg", $item.imageUrl)
                if !item.imageUrl.isEmptyW {
                    WebImage(imageURL: item.imageUrl, isLoaded: $isLoaded)
                        .scaledToFill()
                        .frame(width: 200)
                }
                Text("Publishing Date")
                    .fontWeight(.medium)
                    .font(.footnote)
                DatePicker("", selection: $item.publishingDate, displayedComponents: .date)
                    .datePickerStyle(.automatic)
                    .labelsHidden()
                OneSearchView<PublisherViewModel>(title: "Publisher", example: "ex. Penguin Random House", model: PublisherViewModel(), itemStringValue: .constant(nil), itemIntValue: $item.publisherId)
                OneSearchView<LanguageViewModel>(title: "Language", example: "ex. English", model: LanguageViewModel(), itemStringValue: $item.language, itemIntValue: .constant(nil)) 
                getTextField("Price", "ex. 11.99", $item.price)
                getTextField("Amount In Stock", "ex. 12", $item.amountInStock)
                HorizontalRadioButtons(selectedOption: $item.itemType)
                MultiSearchView<AuthorViewModel>(title: "Authors", example: "ex. Lisa Genova", model: AuthorViewModel(), itemIdArray: $item.authorsIds)
                MultiSearchView<GenreViewModel>(title: "Genres", example: "ex. Medical Drama", model: GenreViewModel(), itemIdArray: $item.genresIds)
                // genres
                if item.itemType == .book {
                    getTextField("Number Of Pages", "ex. 431", $item.numberOfPages)
                }
                // cover Type
                
                Button("Add") {
                    print(item.authorsIds)
                    print(item.genresIds)
                }
            }
            .padding()
        }
    }
    
    func getTextField(_ title: String, _ example: String, _ text: Binding<String>) -> some View {
        VStack(alignment: .leading) {
            Text(title)
                .fontWeight(.medium)
                .font(.footnote)
            TextField(example, text: text)
            if addButtonPressed && text.wrappedValue.isEmptyW {
                Text("􀁟 \(title) cannot be empty!")
                    .foregroundStyle(.red)
                    .font(.footnote)
            } else if addButtonPressed && !isLoaded {
                Text("􀁟 Invalid image URL")
                    .foregroundStyle(.red)
                    .font(.footnote)
            }
        }
    }
    
}

#Preview {
    AddNewItem()
}
