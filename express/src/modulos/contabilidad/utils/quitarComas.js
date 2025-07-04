// Función para quitar comas de números
const quitarComas = (valor) => {
    return parseFloat(valor.replace(/,/g, ''));
  };

  module.exports = {
    quitarComas
  }