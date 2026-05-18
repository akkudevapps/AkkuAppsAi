// AkkuAppsAi Home Page – Universal Language + Theme Toggle
(function() {
    // ---------- TRANSLATIONS (English & Tamil) ----------
    const translations = {
        en: {
            heroBadge: "⚡ AI Powered Solutions",
            heroTitle1: "AI for Everyone –",
            heroTitle2: "Simple, Powerful, Accessible",
            heroDesc: "We bring cutting‑edge artificial intelligence to students, creators, and businesses. Free tools, premium services, and custom AI solutions – all in one place.",
            startChat: "Start Chatting",
            tryEditor: "Try Image Editor",
            servicesTitle: "✨ What We Offer",
            servicesSub: "Free & paid – choose what fits your needs",
            serviceChatTitle: "AI ChatBot",
            serviceChatDesc: "Multi‑model support (GPT, Flux, Llama). Smart, contextual, and always learning.",
            serviceImageTitle: "Image Generator & Editor",
            serviceImageDesc: "Generate AI art, remove backgrounds, upscale, inpaint – professional tools.",
            serviceOcrTitle: "OCR & Document Scanner",
            serviceOcrDesc: "Extract text from images, PDFs, and handwritten notes. Converts to editable text.",
            serviceTypingTitle: "Unicode Typing Tools",
            serviceTypingDesc: "Tamil, English, and other Indic language typing made easy. Windows apps available.",
            serviceDevTitle: "Web & App Development",
            serviceDevDesc: "Custom websites, student projects, social media platforms – we build your vision.",
            serviceMlTitle: "AI/ML Learning & Assistant",
            serviceMlDesc: "Learn machine learning with us. Build your own answering machine / work assistant.",
            tryNow: "Try now →",
            learnMore: "Learn more →",
            contactUs: "Contact us →",
            stayTuned: "Stay tuned →",
            whyTitle: "Why AkkuAppsAi?",
            feat1: "Cutting‑edge models (Flux, GPT, LLaMA)",
            feat2: "Coin system – fair and transparent",
            feat3: "Full Tamil & English support",
            feat4: "Privacy focused, no hidden logs",
            ctaTitle: "Ready to build your AI project?",
            ctaDesc: "From answering machines to complete business solutions – we're here to help.",
            emailUs: "Email Us",
            dashboard: "Dashboard"
        },
        ta: {
            heroBadge: "⚡ செயற்கை நுண்ணறிவு ஆதரவு",
            heroTitle1: "அனைவருக்கும் AI –",
            heroTitle2: "எளிமையானது, சக்திவாய்ந்தது, அணுகக்கூடியது",
            heroDesc: "மாணவர்கள், படைப்பாளிகள், வணிகங்களுக்கு செயற்கை நுண்ணறிவு. இலவச கருவிகள், பிரீமியம் சேவைகள், தனிப்பயன் AI தீர்வுகள் – அனைத்தும் ஒரே இடத்தில்.",
            startChat: "அரட்டை தொடங்குக",
            tryEditor: "பட எடிட்டரை முயல்க",
            servicesTitle: "✨ எங்கள் சேவைகள்",
            servicesSub: "இலவசம் & கட்டணம் – உங்கள் தேவைக்கேற்ப",
            serviceChatTitle: "AI உரையாடல்",
            serviceChatDesc: "பல மாதிரிகள் (GPT, Flux, Llama). நுண்ணறிவு, சூழல் உணர்வு.",
            serviceImageTitle: "பட உருவாக்கி & எடிட்டர்",
            serviceImageDesc: "AI கலை உருவாக்கு, பின்னணி நீக்கு, பெரிதாக்கு – தொழில்முறை கருவிகள்.",
            serviceOcrTitle: "OCR & ஆவண ஸ்கேனர்",
            serviceOcrDesc: "படம், PDF, கையெழுத்திலிருந்து உரையை எடுக்கும். திருத்தக்கூடிய உரையாக மாற்றும்.",
            serviceTypingTitle: "யுனிகோட் தட்டச்சு கருவிகள்",
            serviceTypingDesc: "தமிழ், ஆங்கிலம் மற்றும் இந்திய மொழிகள். விண்டோஸ் பயன்பாடுகள்.",
            serviceDevTitle: "வலை & பயன்பாட்டு உருவாக்கம்",
            serviceDevDesc: "தனிப்பயன் வலைத்தளங்கள், மாணவர் திட்டங்கள், சமூக ஊடகங்கள்.",
            serviceMlTitle: "AI/ML கற்றல் & உதவியாளர்",
            serviceMlDesc: "எங்களுடன் இயந்திர கற்றல் கற்றுக்கொள்ளுங்கள். பதில் இயந்திரம் / பணி உதவியாளர் உருவாக்குக.",
            tryNow: "இப்போது முயல்க →",
            learnMore: "மேலும் அறிக →",
            contactUs: "தொடர்பு கொள்ள →",
            stayTuned: "விரைவில் →",
            whyTitle: "ஏன் AkkuAppsAi?",
            feat1: "நவீன மாதிரிகள் (Flux, GPT, LLaMA)",
            feat2: "நாணய முறை – நியாயமானது, வெளிப்படையானது",
            feat3: "முழு தமிழ் & ஆங்கில ஆதரவு",
            feat4: "தனியுரிமை மையம், மறைக்கப்பட்ட பதிவுகள் இல்லை",
            ctaTitle: "உங்கள் AI திட்டத்தை உருவாக்க தயாரா?",
            ctaDesc: "பதில் இயந்திரங்கள் முதல் முழு வணிக தீர்வுகள் வரை – நாங்கள் உதவ தயாராக உள்ளோம்.",
            emailUs: "மின்னஞ்சல் அனுப்பு",
            dashboard: "டாஷ்போர்டு"
        }
    };

    let currentLang = localStorage.getItem('akku_lang') || 'en';

    // Apply language to all elements with data-lang-key
    function applyLanguage(lang) {
        const elements = document.querySelectorAll('[data-lang-key]');
        elements.forEach(el => {
            const key = el.getAttribute('data-lang-key');
            if (translations[lang] && translations[lang][key]) {
                if (el.tagName === 'INPUT' || el.tagName === 'TEXTAREA') {
                    el.placeholder = translations[lang][key];
                } else {
                    el.innerHTML = translations[lang][key];
                }
            }
        });
        currentLang = lang;
        localStorage.setItem('akku_lang', lang);
        updateLangToggleUI();
    }

    function updateLangToggleUI() {
        const btn = document.getElementById('langToggle');
        if (btn) btn.textContent = currentLang === 'en' ? 'தமிழ்' : 'English';
    }

    // Theme handling (dark/light) – uses existing CSS variables
    function initTheme() {
        const savedTheme = localStorage.getItem('akku_theme') || 'dark';
        document.documentElement.setAttribute('data-theme', savedTheme);
        updateThemeToggleUI(savedTheme);
    }

    function toggleTheme() {
        const current = document.documentElement.getAttribute('data-theme');
        const newTheme = current === 'dark' ? 'light' : 'dark';
        document.documentElement.setAttribute('data-theme', newTheme);
        localStorage.setItem('akku_theme', newTheme);
        updateThemeToggleUI(newTheme);
    }

    function updateThemeToggleUI(theme) {
        const btn = document.getElementById('themeToggle');
        if (btn) {
            btn.innerHTML = theme === 'dark' ? '<i class="fas fa-sun"></i> Light' : '<i class="fas fa-moon"></i> Dark';
        }
    }

    // Add toggle buttons to the page (assuming your _Layout has a top bar)
    // If not, we inject them dynamically (optional)
    function injectToggles() {
        // Check if toggles already exist in the layout; if not, append to top bar
        if (!document.getElementById('langToggle')) {
            const topBar = document.querySelector('.top-bar') || document.querySelector('.appbar') || document.body;
            const langBtn = document.createElement('button');
            langBtn.id = 'langToggle';
            langBtn.className = 'top-btn lang-btn';
            langBtn.style.marginLeft = '8px';
            langBtn.onclick = () => applyLanguage(currentLang === 'en' ? 'ta' : 'en');
            topBar.appendChild(langBtn);
            updateLangToggleUI();
        }
        if (!document.getElementById('themeToggle')) {
            const topBar = document.querySelector('.top-bar') || document.querySelector('.appbar') || document.body;
            const themeBtn = document.createElement('button');
            themeBtn.id = 'themeToggle';
            themeBtn.className = 'top-btn theme-btn';
            themeBtn.onclick = toggleTheme;
            topBar.appendChild(themeBtn);
            const savedTheme = localStorage.getItem('akku_theme') || 'dark';
            updateThemeToggleUI(savedTheme);
        }
    }

    // Export for global use (if needed)
    window.akkuLang = { applyLanguage, currentLang: () => currentLang };
    window.akkuTheme = { toggleTheme };

    // Initialize
    document.addEventListener('DOMContentLoaded', () => {
        initTheme();
        injectToggles();
        applyLanguage(currentLang);
    });
})();