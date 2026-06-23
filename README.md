# VL.Prometheus

Instrument your vvvv apps with Prometheus metrics. Based on [prometheus-net](https://github.com/prometheus-net/prometheus-net).

<p align="center">
<img src="img/logo.png" title="" alt="VL.Prometheus logo" width="256">
</p>

## What the Prometheus

[Prometheus](https://prometheus.io/) is an open-source monitoring solution for your applications. It allows you to store _metrics_ reported by your application over time and query them. You can then visualize these metrics with [Grafana](https://grafana.com/oss/grafana/).

## What this plugin does

This plugin creates a web-server that exposes your metrics in the [Prometheus format](https://prometheus.io/docs/concepts/data_model/) and allows you to easily expose new metrics (your patch framerate, or any arbitrary piece of data relevant to your application).