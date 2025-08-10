struct AgeCategory: Identifiable, Decodable, Hashable {
    var id: Int
    var tag: String
    var description: String
    var minimumAge: Int
}
