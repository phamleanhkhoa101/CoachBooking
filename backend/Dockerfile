FROM gradle:9.1.0-jdk25 AS build
USER root
RUN mkdir -p /app && chown gradle:gradle /app
USER gradle
WORKDIR /app
COPY --chown=gradle:gradle settings.gradle build.gradle ./
RUN gradle --no-daemon dependencies
COPY --chown=gradle:gradle src ./src
RUN gradle --no-daemon bootJar

FROM eclipse-temurin:25-jre-alpine
WORKDIR /app
RUN addgroup -S app && adduser -S app -G app
COPY --from=build /app/build/libs/*.jar app.jar
USER app
EXPOSE 8080
ENTRYPOINT ["java", "-jar", "app.jar"]
