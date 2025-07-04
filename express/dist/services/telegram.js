"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __classPrivateFieldGet = (this && this.__classPrivateFieldGet) || function (receiver, state, kind, f) {
    if (kind === "a" && !f) throw new TypeError("Private accessor was defined without a getter");
    if (typeof state === "function" ? receiver !== state || !f : !state.has(receiver)) throw new TypeError("Cannot read private member from an object whose class did not declare it");
    return kind === "m" ? f : kind === "a" ? f.call(receiver) : f ? f.value : state.get(receiver);
};
var _TelegramBot_instances, _TelegramBot_handleResponse, _TelegramBot_fetchTelegram;
class TelegramBot {
    constructor() {
        _TelegramBot_instances.add(this);
        this.botToken = process.env.TELEGRAM_BOT_TOKEN;
        this.apiUrl = `https://api.telegram.org/bot${this.botToken}`;
    }
    enviarMensajeConUbicacion(chatId, mensaje, latitud, longitud) {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                console.log("Token:", process.env.TELEGRAM_BOT_TOKEN); // Para debug
                console.log("ChatId:", chatId);
                if (!chatId)
                    throw new Error("Chat ID es requerido");
                if (!mensaje)
                    throw new Error("Mensaje es requerido");
                if (!latitud || !longitud)
                    throw new Error("Latitud y longitud son requeridos");
                const [mensajeRes, ubicacionRes] = yield Promise.all([
                    __classPrivateFieldGet(this, _TelegramBot_instances, "m", _TelegramBot_fetchTelegram).call(this, "sendMessage", {
                        chat_id: chatId,
                        text: mensaje,
                        parse_mode: "HTML",
                    }),
                    __classPrivateFieldGet(this, _TelegramBot_instances, "m", _TelegramBot_fetchTelegram).call(this, "sendLocation", {
                        chat_id: chatId,
                        latitude: latitud,
                        longitude: longitud,
                    }),
                ]);
                return {
                    success: true,
                    message: "Mensaje y ubicación enviados correctamente",
                    data: {
                        mensaje: mensajeRes.result,
                        ubicacion: ubicacionRes.result,
                    },
                };
            }
            catch (error) {
                console.error("Error detallado:", error);
                return {
                    success: false,
                    message: error.message || "Error desconocido",
                    timestamp: new Date().toISOString(),
                };
            }
        });
    }
    enviarMensaje(chatId, mensaje) {
        return __awaiter(this, void 0, void 0, function* () {
            return __classPrivateFieldGet(this, _TelegramBot_instances, "m", _TelegramBot_fetchTelegram).call(this, "sendMessage", {
                chat_id: chatId,
                text: mensaje,
                parse_mode: "HTML",
            });
        });
    }
    enviarMensajeMasivo(mensaje, chatIds) {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const promesasEnvio = chatIds.map((chatId) => __classPrivateFieldGet(this, _TelegramBot_instances, "m", _TelegramBot_fetchTelegram).call(this, "sendMessage", {
                    chat_id: chatId,
                    text: mensaje,
                    parse_mode: "HTML",
                }).catch((error) => {
                    console.error(`Error enviando mensaje a ${chatId}:`, error.message);
                    // Retornamos un objeto con error en lugar de null
                    return { error: true, chatId, mensaje: error.message };
                }));
                const resultados = yield Promise.allSettled(promesasEnvio);
                // Mejoramos la detección de errores
                const exitosos = resultados.filter((r) => { var _a; return r.status === "fulfilled" && !((_a = r.value) === null || _a === void 0 ? void 0 : _a.error); }).length;
                const fallidos = resultados.filter((r) => { var _a; return r.status === "rejected" || ((_a = r.value) === null || _a === void 0 ? void 0 : _a.error); }).length;
                return {
                    success: exitosos > 0 && fallidos === 0, // Solo es exitoso si no hay fallos
                    mensaje: `Mensaje enviado a ${exitosos} usuarios. Fallos: ${fallidos}`,
                    detalles: {
                        total: chatIds.length,
                        exitosos,
                        fallidos,
                    },
                    error: fallidos > 0 ? "Algunos mensajes no pudieron ser enviados" : null,
                };
            }
            catch (error) {
                console.error("Error en envío masivo:", error);
                return {
                    success: false,
                    mensaje: "Error al enviar mensajes masivos",
                    error: error.message,
                    detalles: {
                        total: chatIds.length,
                        exitosos: 0,
                        fallidos: chatIds.length,
                    },
                };
            }
        });
    }
}
_TelegramBot_instances = new WeakSet(), _TelegramBot_handleResponse = function _TelegramBot_handleResponse(response) {
    return __awaiter(this, void 0, void 0, function* () {
        if (!response.ok) {
            const errorData = yield response.json();
            throw new Error(`API Error: ${errorData.description}`);
        }
        return response.json();
    });
}, _TelegramBot_fetchTelegram = function _TelegramBot_fetchTelegram(endpoint, body) {
    return __awaiter(this, void 0, void 0, function* () {
        const response = yield fetch(`${this.apiUrl}/${endpoint}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body),
        });
        return __classPrivateFieldGet(this, _TelegramBot_instances, "m", _TelegramBot_handleResponse).call(this, response);
    });
};
module.exports = new TelegramBot();
