# Dashboard Management System - Back-End

این پروژه مربوط به قسمت **Back-End** یک وبسایت داشبورد مدیریتی است که برای مدیران کارگزاری‌های بورس طراحی شده است. هدف این سیستم مدیریت و نظارت بر عملکرد و وضعیت کارگزاری‌ها و ارائه گزارش‌ها و آمار به مدیران است.

## توضیحات

پروژه‌ی **Dashboard Management** به مدیران این امکان را می‌دهد که بر عملکرد کارگزاری‌های بورس نظارت داشته باشند. از طریق این داشبورد، مدیران می‌توانند آمار و گزارشات مختلف را مشاهده کنند و در صورت نیاز، اقداماتی را انجام دهند.

این پروژه به عنوان **Back-End** پیاده‌سازی شده است و شامل APIهایی است که برای ارتباط با دیتابیس و انجام عملیات‌های مدیریتی مختلف طراحی شده‌اند.

## ویژگی‌ها

- **احراز هویت و مجوزها**: تنها کاربران دارای مجوز مناسب می‌توانند به منابع خاص دسترسی داشته باشند.
- **مدیریت کاربران**: مدیران می‌توانند کاربران جدید اضافه کرده، ویرایش کنند و حذف نمایند.
- **گزارشات و آمار**: ارائه گزارشات متنوع از عملکرد کارگزاری‌ها و اطلاعات لحظه‌ای.
- **پشتیبانی از چندین کارگزاری**: پشتیبانی از چندین کارگزاری با قابلیت مشاهده و مدیریت داده‌های هرکدام به صورت جداگانه.

## تکنولوژی‌های استفاده‌شده

- **C#** (برای توسعه APIها)
- **ASP.NET Core** (برای ساخت وب API)
- **Entity Framework Core** (برای ارتباط با دیتابیس)
- **SQL Server** (برای پایگاه داده)
- **JWT (JSON Web Tokens)** (برای احراز هویت و مجوز)
- **Swagger** (برای مستندات API)

## راه‌اندازی پروژه

### پیش‌نیازها

- .NET 6.0 یا بالاتر
- SQL Server یا پایگاه داده مشابه
- Swagger برای مشاهده مستندات API (اختیاری)

### مراحل نصب

1. **کلون کردن ریپازیتوری**:

   ```bash
   git clone https://github.com/yourusername/yourrepository.git
