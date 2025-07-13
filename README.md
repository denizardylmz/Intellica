# 🤖 Intellica - Telegram-Based Crypto Insight Bot

A modular, command-based Telegram chatbot that provides real-time crypto insights, realized PnL tracking, and extensible support for AI-enhanced services like weather forecasts and note management.

---

## 🔍 Overview

This project is designed as a backend-focused chatbot service powered by Telegram.  
It focuses primarily on **cryptocurrency portfolio tracking**, but is structured to easily support new commands like weather queries, note-taking, and AI conversation endpoints.

---

## 📦 Features

### 🧩 Modular Command System
- Each command is a class implementing `ICommandHandler`
- Commands are auto-registered at runtime using `[TelegramCommand]` attributes
- Enabled/disabled via `appsettings.json` for full flexibility

### 📊 Realized Profit/Loss Tracking
- Interacts with Binance API
- Fetches **realized PnL** per symbol or for all symbols

### 🧠 LLM-Ready (Planned)
- Weather and crypto summaries via local or external LLMs
- Natural language support for queries and summaries

### ☁️ Weather Forecasting (Coming Soon)
- Will support weather queries and summaries by location, powered by OpenWeather

---

## 💬 Sample Commands

```shell
/total BTCUSDT 7         → Realized PnL of BTC in last 7 days
/total help              → Shows help for total command
/getprice ETHUSDT        → Current price and indicators
