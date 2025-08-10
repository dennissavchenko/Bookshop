import SwiftUI

struct ItemListView: View {
    
    var orderItem: OrderItem
    @State var isLoaded: Bool = false
    
    var body: some View {
        HStack(alignment: .top, spacing: 12) {
            WebImage(imageURL: orderItem.item.imageUrl, isLoaded: $isLoaded)
                .scaledToFit()
                .frame(width: 60, height: 90, alignment: .topLeading)
                .shadow(radius: 6)
            VStack(alignment: .leading) {
                Text(orderItem.item.name)
                    .fontWeight(.semibold)
                Text(orderItem.item.authors?.joined(separator: ", ") ?? orderItem.item.publisherName)
                    .foregroundStyle(.gray)
                    .font(.footnote)
            }
            Spacer()
            VStack(alignment: .trailing) {
                HStack(spacing: 4) {
                    Text(orderItem.item.price.priceFormat)
                        .fontWeight(.bold)
                    Text("x \(orderItem.quantity)")
                        .foregroundStyle(.gray)
                        .italic()
                }
                Spacer()
                HStack(spacing: 4) {
                    Image(systemName: "star.fill")
                    Text(String(format: "%.1f", orderItem.item.averageRating))
                        .fontWeight(.semibold)
                }
                .font(.footnote)
                .foregroundStyle(Color.star)
            }
        }
        .frame(height: 90)
    }
}

#Preview {
    ItemListView(orderItem: OrderItem(item: SimpleItem(id: 1, name: "Wool", imageUrl: "https://m.media-amazon.com/images/I/81qFq9nLjzL._AC_UF1000,1000_QL80_.jpg", price: 12.99, publisherName: "HMH Books", averageRating: 4.9, authors: ["Hugh Howey"], genres: ["Science Fiction"]), quantity: 2))
}
