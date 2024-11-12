const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:30236';

const context = [
    "/api/bitacora",
    "/api/backup",
    "/api/login",
    "/api/session",
    "/api/products",
    "/api/test",
<<<<<<< HEAD
    "/api/language",
    "/api/recalculardv",
=======
    "/api/recalculardv",
    "/api/permiso"
>>>>>>> 90b561b152c53650478b08319d5a8450f69c8f7f
];

module.exports = function(app) {
  const appProxy = createProxyMiddleware(context, {
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  });

  app.use(appProxy);
};
