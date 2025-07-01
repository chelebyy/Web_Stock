/// <reference lib="webworker" />

addEventListener('message', ({ data }) => {
  // Simulate a very heavy computation
  const start = Date.now();
  let count = 0;
  // This loop will run for a few seconds, blocking the worker thread, but not the UI thread.
  for (let i = 0; i < 10_000_000_000; i++) {
    count++;
  }
  const end = Date.now();
  const duration = (end - start);

  const response = `Ağır hesaplama tamamlandı. Süre: ${duration}ms`;
  postMessage(response);
});
