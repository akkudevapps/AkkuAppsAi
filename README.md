# 🔋 AkkuAppsAi

**ASP.NET Core 8 MVC** மாதிரியில் உருவாக்கப்பட்ட AI-ஊக்க இணையப் பயன்பாடு.  
📍 உள்ளூர் முகவரி: `https://localhost:7010`

---

## 🗂️ திட்டக் கட்டமைப்பு (Project Architecture)

```text
AkkuAppsAi/                     # ரூட் கோப்புறை
├── AkkuAppsAi/                 # 🔥 முதன்மை MVC இயக்கக் கோப்புறை
│   ├── Controllers/            # HTTP கோரிக்கைகளைக் கையாள்கிறது
│   │   └── HomeController.cs
│   ├── Models/                 # தரவு வடிவமைப்புகள்
│   │   └── ErrorViewModel.cs
│   ├── Views/                  # காட்சிக் கோப்புகள் (Razor)
│   │   ├── Home/               # முகப்புப் பக்கக் காட்சிகள்
│   │   ├── Shared/             # பொதுவான _Layout.cshtml
│   │   ├── _ViewImports.cshtml
│   │   └── _ViewStart.cshtml
│   ├── wwwroot/                # நிலையான கோப்புகள்
│   │   ├── css/ (site.css, home.css)
│   │   ├── js/  (site.js, home.js)
│   │   └── lib/ (bootstrap, jquery)
│   ├── Program.cs              # .NET 8 நுழைவாயில் & சேவைகள் பதிவு
│   ├── appsettings.json        # கட்டமைப்புகள் (இணைப்புச் சரம், log)
│   ├── appsettings.Development.json
│   ├── AkkuAppsAi.csproj       # NuGet சார்புகள்
│   └── Properties/
│       └── launchSettings.json # 🔑 `https://localhost:7010` – இங்கே!
├── .gitignore, .gitattributes
├── AkkuAppsAi.slnx             # Visual Studio தீர்வு
├── LICENSE.txt (MIT)
└── README.md (இந்தக் கோப்பு)