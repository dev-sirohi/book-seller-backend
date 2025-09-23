src/
│
├── Api/
│   ├── Controllers/
│   │   ├── LandingController.cs
│   │   ├── SupportController.cs
│   │   ├── CollectionsController.cs
│   │   ├── PublisherController.cs
│   │   ├── SettingsController.cs
│   │   └── AuthController.cs
│   └── Program.cs
│
├── Application/
│   ├── DTOs/
│   │   ├── BookDto.cs
│   │   ├── AuthorDto.cs
│   │   ├── User.cs
│   │   └── PurchaseDto.cs
│   ├── Interfaces/
│   │   ├── IBookService.cs
│   │   ├── IUserService.cs
│   │   ├── IPublisherService.cs
│   │   └── IPaymentService.cs
│   └── Services/
│       ├── BookService.cs
│       ├── UserService.cs
│       ├── PublisherService.cs
│       └── PaymentService.cs
│
├── Domain/
│   ├── Entities/
│   │   ├── Book.cs
│   │   ├── Author.cs
│   │   ├── User.cs
│   │   ├── Publisher.cs
│   │   ├── Purchase.cs
│   │   └── Bookmark.cs
│   ├── Enums/
│   │   ├── UserRole.cs
│   │   ├── Genre.cs
│   │   └── Language.cs
│   └── ValueObjects/
│       └── Money.cs
│
├── Infrastructure/
│   ├── Data/
│   │   ├── AppDbContext.cs
│   │   └── Migrations/
│   ├── Repositories/
│   │   ├── BookRepository.cs
│   │   ├── UserRepository.cs
│   │   └── PublisherRepository.cs
│   ├── Payment/
│   │   └── PaytmGateway.cs
│   └── Services/
│       └── EmailService.cs
│
├── Shared/
│   ├── Constants/
│   │   └── AppConstants.cs
│   ├── Helpers/
│   │   └── Utils.cs
│   └── Extensions/
│       └── QueryableExtensions.cs
│
└── Tests/
    ├── Unit/
    └── Integration/