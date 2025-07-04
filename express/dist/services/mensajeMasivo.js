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
var _TelegramAPI_instances, _TelegramAPI_handleResponse, _TelegramAPI_fetchTelegram;
class TelegramAPI {
    constructor() {
        _TelegramAPI_instances.add(this);
        this.botToken = "7561647559:AAHuAYYJDl2kjJZ9qwYg2tckHiKfTXK40cg";
        this.apiUrl = `https://api.telegram.org/bot${this.botToken}`;
    }
    enviarMensajeMasivo(mensaje, chatIds) {
        return __awaiter(this, void 0, void 0, function* () {
            try {
                const promesasEnvio = chatIds.map((chatId) => __classPrivateFieldGet(this, _TelegramAPI_instances, "m", _TelegramAPI_fetchTelegram).call(this, "sendMessage", {
                    chat_id: chatId,
                    text: mensaje,
                    parse_mode: "HTML",
                }).catch((error) => {
                    console.error(`Error enviando mensaje a ${chatId}:`, error);
                    return { error: true, chatId, mensaje: error.message };
                }));
                const resultados = yield Promise.allSettled(promesasEnvio);
                const exitosos = resultados.filter((r) => { var _a; return r.status === "fulfilled" && !((_a = r.value) === null || _a === void 0 ? void 0 : _a.error); }).length;
                const fallidos = resultados.filter((r) => { var _a; return r.status === "rejected" || ((_a = r.value) === null || _a === void 0 ? void 0 : _a.error); }).length;
                return {
                    success: exitosos > 0,
                    mensaje: `Mensaje enviado a ${exitosos} usuarios. Fallos: ${fallidos}`,
                    detalles: {
                        total: chatIds.length,
                        exitosos,
                        fallidos,
                    },
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
_TelegramAPI_instances = new WeakSet(), _TelegramAPI_handleResponse = function _TelegramAPI_handleResponse(response) {
    return __awaiter(this, void 0, void 0, function* () {
        if (!response.ok) {
            const errorData = yield response.json();
            throw new Error(`API Error: ${errorData.description}`);
        }
        return response.json();
    });
}, _TelegramAPI_fetchTelegram = function _TelegramAPI_fetchTelegram(endpoint, body) {
    return __awaiter(this, void 0, void 0, function* () {
        const response = yield fetch(`${this.apiUrl}/${endpoint}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(body),
        });
        return __classPrivateFieldGet(this, _TelegramAPI_instances, "m", _TelegramAPI_handleResponse).call(this, response);
    });
};
// Instanciamos la clase
const telegram = new TelegramAPI();
// Lista de chat_ids
const chatIds = [7916186377, 8172917124, 7554800275, 5691578531];
const mensaje = `🚨 <b>Mensaje de Prueba</b>

¡Hola a todos! Soy el bot ayudante programado para dar apoyo a Logistica, estoy trabajando para mejorar el sistema de notificaciones asegurandome de que funcione correctamente!.

Saludos.`;
// Verificación del token
console.log("Bot Token:", "Configurado");
console.log("Intentando enviar mensaje a:", chatIds);
console.log("URL del bot:", `https://api.telegram.org/bot${telegram.botToken.substring(0, 5)}...`);
telegram
    .enviarMensajeMasivo(mensaje, chatIds)
    .then((resultado) => {
    if (!resultado.success || resultado.detalles.fallidos > 0) {
        console.log("\n❌ Error: Los mensajes no fueron enviados");
        console.log(`✅ Enviados: ${resultado.detalles.exitosos}`);
        console.log(`❌ Fallidos: ${resultado.detalles.fallidos}`);
        if (resultado.error)
            console.log("Error:", resultado.error);
    }
    else {
        console.log("\n✅ Todos los mensajes fueron enviados exitosamente");
    }
    console.log("\nResultado detallado:", resultado);
})
    .catch((error) => {
    console.error("\n❌ Error general:", error.message);
});
