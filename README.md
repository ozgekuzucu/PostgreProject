# 🍰 BakeryAI – PostgreSQL Tabanlı Yapay Zeka Destekli Pastane Platformu

BakeryAI, PostgreSQL altyapısıyla çalışan, tek sayfalık dinamik bir pastane websitesi ve gelişmiş bir Admin Paneli içeren yapay zeka destekli bir içerik yönetim platformudur. 
Proje; Hugging Face API ve Machine Learning Forecasting teknolojilerini bir araya getirerek modern, tamamen dinamik bir mimari sunar.

## 🚀 Proje Özeti

Proje, PostgreSQL veritabanı ile dinamik olarak çalışır. Kullanılan entity'ler:

| Entity | Açıklama |
|--------|----------|
| Slider | Anasayfa üst slider içerikleri |
| About | Hakkımızda alanı, AI ile otomatik yazı üretimi |
| Category | Ürün kategorileri |
| Product | Ürün listesi ve detayları |
| Services | Sunulan hizmetler |
| Chefs | Şefler / çalışanlar |
| Testimonial | Müşteri yorumları |
| Order | Sipariş kayıtları |

## 🛠 Kullanılan Teknolojiler

- PostgreSQL (UI + Admin Panel dinamik veri)
- Entity Framework 6.2 – DB First
- ASP.NET Core 9.0
- Bootstrap 5 – Bakery Template (Baker)
- SignalR (Gerçek zamanlı chatbot)
- Hugging Face API
- Machine Learning Forecasting (ML.NET)

## 🎨 Tema Özellikleri

**Kullanılan tema:** Bootstrap 5 – Bakery Website Template (Baker)

Tüm içerikler Admin Panel üzerinden dinamik olarak yönetilir.

Tema üzerindeki Call Us butonu tıklandığında kullanıcı WhatsApp API ile direkt WhatsApp mesaj ekranına yönlendirilir.

## 🤖 Yapay Zeka Entegrasyonları (Hugging Face)

📌 **About Alaının Yapay Zeka ile Üretilmesi**

Admin panelindeki About alanı için Hugging Face'in Meta-Llama-3-8B-Instruct modeli kullanılır. Yönetici, tek tuşla "Yazıyı Oluştur" diyerek yapay zekadan otomatik içerik oluşturabilir.

📌 **Ürün Detaylarının Yapay Zeka ile Oluşturulması**

Product detay sayfasında yer alacak açıklamalar Hugging Face AI ile otomatik üretilir. Admin, sadece ürün adını girer → AI kısa, etkileyici ve doğal bir açıklama yazar.

📌**Gerçek Zamanlı Admin Chatbot (Hugging Face + SignalR)**

Admin paneline entegre SignalR tabanlı bir canlı sohbet sistemi bulunmaktadır.
- Admin sayfasında gerçek zamanlı soru → anında yanıt
- Müşteri hizmetleri asistanı rolünde çalışır.

📌 **Tarif Önerisi (AI Recipe Generator)**

Admin panelinde ürünün malzemeleri girildiğinde yapay zeka otomatik 3 farklı tarif önerir:

Özellikler:
- Malzemeler analiz edilir.
- 3 farklı tatlı tarifi üretilir.
- Her tarif için: malzeme listesi, hazırlanış adımları, süre, porsiyon bilgisi ve ipuçları
- Düzenli format ile sunulur.

## 📂 10.000 Satırlık Sipariş Verisi

Tüm sipariş verisi ChatGPT ile oluşturulmuş, CSV formatında hazırlanıp projeye dahil edilmiştir. ImportOrders sayfasından PostgreSQL'e import edilir.

## 📊 9 Aylık Periyot + Sonraki 3 Ay Tahmin

Ocak–Eylül arası veri analiz edilir. Ekim–Kasım–Aralık ayları için Machine Learning Forecasting ile satış tahmini yapılır. Tahmin grafikleri Admin Dashboard'da gösterilir.

## 📈 Analitik & Satış Tahminleme

Admin panelinde gelişmiş analitik ekranlar bulunmaktadır:

🔥 En Çok Satan Ürün Analizi (Son 6 Ay)

- Son 6 aylık siparişler incelenir
- En çok satan ürünler sıralanır
- Trend analizi yapılır
- Grafiksel olarak raporlanır

## 📊 Satış Trend Forecasting

- ML modeli 9 aylık veriyi öğrenir
- Sonraki 3 ay için tahmin üretir
- Tahmin sonuçları dashboard üzerinde gösterilir

## 📸 Uygulama Görselleri

**🏠 Ana Sayfa**

![Ana Sayfa](/images/homepage.jpeg)

**🧑‍💼 Admin Paneli - Dashboard**

![Admin Dashboard](/images/dashboard.jpeg)

**🤖 About Alanı**

![AI About](/images/about-edit.jpeg)

![AI About](/images/about-list.png)

**📝 Ürün Alanı**

![AI Product Description](/images/product-list.jpeg)

![AI Product Description](/images/product-edit.jpeg)

**💬 Gerçek Zamanlı Chatbot (SignalR + Hugging Face)**

![Chatbot](/images/ai-chatbot.jpeg)

**🍰 AI Tarif Önerisi**

![Recipe Generator](/images/ai-recipe.jpeg)

**🛒 Sipariş Listesi ve Ekleme**

![Orders](/images/order-list.jpeg)

![Orders](/images/import-orders.jpeg)

**📋 Kategoriler**

![Kategoriler](/images/category-list.png)

![Kategoriler](/images/category-add.png)

**🗺️ Servisler**

![Servisler](/images/service-list.png)

![Servisler](/images/service-edit.png)

**👥 Şef Yönetimi**

![Chefs Management](/images/chef-list.png)

![Chefs Management](/images/chef-edit.png)

**💬 Müşteri Yorumları (Testimonials)**

![Testimonials](/images/comment-list.png)

![Testimonials](/images/comment-edit.png)
